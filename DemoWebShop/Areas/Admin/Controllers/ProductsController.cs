using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoWebshop.Data;
using Microsoft.AspNetCore.Authorization;
using DemoWebShop.Models;

namespace DemoWebshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            return _context.Products != null ?
                        View(await _context.Products.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Products'  is null.");
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"] as string ?? "";
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Title,Description,Sku,InStock,Price,Image")] Product product,
            int[] categoryIds,
            IFormFile Image
        )
        {
            // 1. korak -> provjeri ako je parametar categoryIds prazan ili null
            if (categoryIds.Length == 0 || categoryIds == null)
            {
                // Izbaci poruku kako kategorije se moraju odabrati!
                // TempData je svojstvo koja kreira kratkoročne poruke u sesiji između dvije akcije
                TempData["ErrorMessage"] = "Molimo odaberite minimalno jednu kategoriju!";
                return RedirectToAction(nameof(Create));
            }

            // 2. korak -> pohrani proizvod u tablicu i nakon toga poveži proizvod s odabranim kategorijama
            if (ModelState.IsValid)
            {
                // 2.1 korak -> pokušaj pohraniti sliku na disk i spremi naziv slike u svojstvo product.Image
                try
                {
                    // Primjer 1
                    var imageName = Image.FileName.ToLower();

                    // Primjer 2
                    // var imageName = Image.FileName.ToLower().Replace(" ", "").Replace("_", "-");

                    // Primjer 3 - 2023-04-06-18-04-04.jpg
                    // var getFimeExtension = Path.GetExtension(Image.FileName);
                    // var imageName = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + getFimeExtension;

                    // Odabir putanje gdje će slika biti pohranjena
                    // Rezultat: ~/wwwroot/images/products/naziv-slike.jpg 
                    var saveImagePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/images/products",
                        imageName
                    );

                    // Kreiraj direktorije i poddirektorije unutar zadane putanje (wwwroot/images/products)
                    Directory.CreateDirectory(Path.GetDirectoryName(saveImagePath));

                    // Ovdje se datoteka fizički kopira unutar zadane putanje (wwwroot/images/products) direktorija projekta
                    using (
                        var stream = new FileStream(saveImagePath, FileMode.Create)
                    )
                    {
                        Image.CopyTo(stream);
                    }

                    // U stupac tablice pohranjujemo samo naziv datoteke
                    product.Image = imageName;

                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return RedirectToAction(nameof(Create));
                }


                _context.Add(product);
                // _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // Nakon pohrane zapisa u tablicu EF Core će u objektu popuniti vrijednost za svojstvo product.Id

                // 2.2 korak -> poveži product.Id sa stavkama niza categoryIds i pohrani sve u tablicu ProductCategories
                foreach (var categoryId in categoryIds)
                {
                    ProductCategory productCategory = new ProductCategory();
                    productCategory.CategoryId = categoryId;
                    productCategory.ProductId = product.Id;

                    _context.ProductCategories.Add(productCategory);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Dohvati ID-ejeve kategorija s kojima je porizvod povezan u tablici ProductCategories
            ViewBag.ProductCategories = _context.ProductCategories.Where(
                p => p.ProductId == product.Id
                ).Select(
                c => c.CategoryId
                ).ToList();

            // Ako postoji error poruka, spremi u ViewBag svojstvo
            ViewBag.ErrorMessage = TempData["ErrorMessage"] ?? "";

            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Description,Sku,InStock,Price,Image")] Product product,
            IFormFile? newImage,
            int[] categoryIds
        )
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            // Provjeri ako je odabrana barem jedna kategorija
            if (categoryIds.Length == 0)
            {
                TempData["ErrorMessage"] = "Molimo odaberite minimalno jednu kategoriju!";
                return RedirectToAction(nameof(Edit), new { id = id });
            }

            if (ModelState.IsValid)
            {
                try
                {

                    // Provjeri postoji li vrijednost parametra NewImage (znači netko je dodano novu sliku)
                    if (newImage != null)
                    {
                        // Primjer: 2023-04-13-19-16-22_moja_slika1.jpg
                        // Primjer 2: NazivKategorije-NazivProizvoda-IdProizvoda.(jpg, png, ...)
                        // Primjer 3: SKU.(jpg, png, itd...)
                        var newImageName = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + "_" + newImage.FileName.ToLower().Replace(" ", "_");

                        // Odabir putanje gdje će slika biti pohranjena
                        // Rezultat: ~/wwwroot/images/products/naziv-slike.jpg 
                        var saveImagePath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot/images/products",
                            newImageName
                        );

                        // Kreiraj direktorije i poddirektorije unutar zadane putanje (wwwroot/images/products)
                        Directory.CreateDirectory(Path.GetDirectoryName(saveImagePath));

                        // Ovdje se datoteka fizički kopira unutar zadane putanje (wwwroot/images/products) direktorija projekta
                        using (
                            var stream = new FileStream(saveImagePath, FileMode.Create)
                        )
                        {
                            newImage.CopyTo(stream);
                        }

                        // U stupac tablice pohranjujemo samo naziv datoteke
                        product.Image = newImageName;

                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();

                    // Ažuriramo kategorije proizvoda u tablici ProductCategories

                    // 1. izbriši sve postojeće konekcije između kategorije i proizvoda (ako postoje)
                    _context.ProductCategories.RemoveRange(_context.ProductCategories.Where(p => p.ProductId == id));
                    _context.SaveChanges();

                    // 2. ažuriraj nove podatke s vezom između proizvoda i kategorije u tablici ProductCategories
                    foreach (var category in categoryIds)
                    {
                        ProductCategory productCategory = new ProductCategory();
                        productCategory.ProductId = product.Id;
                        productCategory.CategoryId = category;

                        _context.Add(productCategory);
                    }
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }

            // EF -> automatski podešava vanjski ključ na onDelete: Cascade (postoji i onDelete : Restrict)
            // EF briše sve zapise gdje je ID proizvoda vanjski ključ i briše sam zapis proizvoda
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                if(!String.IsNullOrWhiteSpace(product.Image))
                {
                    // putanja na disku servera gdje se slika treba nalaziti
                    // C:/moj-folder/drugi-folder/projekt/wwwroot/images/products/slika.jpg
                    var deleteImageFromPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/images/products",
                        product.Image
                        );

                    //Ako postoji datoteka izbriši je
                    if(System.IO.File.Exists(deleteImageFromPath))
                    {
                        System.IO.File.Delete(deleteImageFromPath);
                    }
                }
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
