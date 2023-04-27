using DemoWebshop.Data;
using DemoWebShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DemoWebshop.Controllers
{
    // Atribut [Authorize] - primjenjuje se na cijeli kontroler ili određene akcije kontrolera
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        // Dependency injection za objekt klase ApplicationDbContext
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index(string? searchQuery, int orderBy = 0, int? categoryId = 0)
        {

            // 1. Po standardu (default) učitaj sve proizvode iz tablice
            List<Product> products = _dbContext.Products.ToList();

            // 2. Ako parametar "categoryId" postoji i nije 0, filtriraj proizvode po kategoriji
            if (categoryId > 0)
            {
                products = products.Where( // popis proizvoda, i postavljanje kriterija
                    p => _dbContext.ProductCategories.Where(
                            pc => pc.CategoryId == categoryId // Ako je u tablici ProductCategories vrijednost stupca CategoryId = categoryId
                        ).Select(
                            pc => pc.ProductId // ako je kriterij zadovoljen, vrati vrijednost stupca productId
                        ).ToList().Contains(
                            p.Id // Nakon toga, vrati objekte klase Product, čiji ID se nalazi u rezultatu kriterija 
                        )
                ).ToList();
            }

            // 3. Ako parametar "searchQuery" postoji i nije prazan, dodatno filtriraj proizvode (pretraži ključnu riječ u naslovu)
            if (!String.IsNullOrWhiteSpace(searchQuery))
            {
                products = products.Where(p => p.Title.ToLower().Contains(searchQuery.ToLower())).ToList();
            }

            /*
             * 0 - zadani prikaz rezultata
             * 1 - sortiranje po naslovu uzlazno
             * 2 - sortiranje po naslovu silazno
             * 3 - sortiranje po cijenu uzlazno
             * 4 - sortiranje po cijenu silazno
             */
            switch (orderBy)
            {
                case 1: products = products.OrderBy(p => p.Title).ToList(); break;
                case 2: products = products.OrderByDescending(p => p.Title).ToList(); break;
                case 3: products = products.OrderBy(p => p.Price).ToList(); break;
                case 4: products = products.OrderByDescending(p => p.Price).ToList(); break;
            }


            // Lista kategorija
            ViewBag.Categories = _dbContext.Categories.ToList();

            ViewBag.ThankYouMessage = TempData["ThankYouMessage"] as string ?? "";

            // U view moramo proslijediti kolekciju proizvoda
            return View(products);

            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
