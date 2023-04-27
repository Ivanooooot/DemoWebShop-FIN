using DemoWebshop.Data;
using DemoWebShop.Services.Cart;
using DemoWebShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebshop.Controllers;

public class CartController : Controller
{
    // Objekt za pristup bazi podataka
    private readonly ApplicationDbContext _dbContext;

    // Ključ naše sesije za košaricu
    public const string sessionCartKey = "_cart";

    public CartController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: Cart/Index
    public IActionResult Index()
    {
        // Korak 1: Provjeri košaricu iz sesije
        List<CartItem> cart = HttpContext.Session.GetCartObjectFromJson(sessionCartKey);

        // Korak 2: Provjeri error poruku
        ViewBag.CartErrorMessage = TempData["CartErrorMessage"] as string ?? "";

        return View(cart);
    }

    // TODO: AddToCart(int productId, decimal quantity)
    // POST: Cart/AddToCart(int productId, decimal quantity)
    [HttpPost]
    public IActionResult AddToCart(int productId, decimal quantity)
    {
        if (quantity <= 0)
        {
            return RedirectToAction(nameof(Index), "Home");
        }

        /*
         * Mogući scenariji
         *  - košarica je prazna
         *      - kreiraj objekt klase CartItem i popuni ga s podacima, dodaj u kolekciju, pa spremi sve u sesiju
         *  - košarica nije prazna
         *      - isti proizvod već postoji, ažurirati količinu, pohrani opet sve u sesiju
         *      - proizvod ne postoji u košarici, dodaj ga, ažururaj sve i dodaj u sesiju
         */

        // Korak 1: provjeri ako postoji proizvod
        var findProduct = _dbContext.Products.Find(productId);
        if (findProduct == null)
        {
            return RedirectToAction(nameof(Index), "Home");
        }

        // Korak 2: Provjeri sesiju
        List<CartItem> cart = HttpContext.Session.GetCartObjectFromJson(sessionCartKey);

        // Korak 3: uvjeti za korištenje košarice
        if (cart.Count == 0)
        {
            // Što ako netko želi više proizvoda nego što ih imamo dostupno?
            if (quantity > findProduct.InStock)
            {
                TempData["CartErrorMessage"] = $"Nije moguće dodati proizvod u košaricu! Na zalihi je dostupno {findProduct.InStock} proizvoda {findProduct.Title}";
                return RedirectToAction(nameof(Index));
            }

            // Kreiraj novi objekt klase CartItem i popuni ga s podacima o proizvodu i količini
            CartItem newItem = new CartItem()
            {
                Product = findProduct,
                Quantity = quantity
            };
            // Dodaj stavku u kolekciju košarice
            cart.Add(newItem);
            // Ažuriraj sesiju za košaricu
            HttpContext.Session.SetCartObjectAsJson(sessionCartKey, cart);
        }
        else
        {
            // Ako proizvod nije u košarici, kreiraj novi objekt klase CartItem, ako je u košarici onda samo ažuriraj količinu tog proizvoda
            var updateOrCreateItem = cart.Find(p => p.Product.Id == productId) ?? new CartItem();

            // Provjera količine 
            // Primjer 1: U košarici imamo 2 soka od jabuke, a InStock = 5.
            // Primjer 2: U košarici nemamo proizvod, a InStock = 3. 
            if (quantity + updateOrCreateItem.Quantity > findProduct.InStock)
            {
                TempData["CartErrorMessage"] = $"Nije moguće dodati odabranu količinu proizvoda. Na zahili je dostupno: {findProduct.InStock} proizvoda {findProduct.Title}";
                return RedirectToAction(nameof(Index));
            }

            // Uvjet za ažuriranje podataka sesije
            if (updateOrCreateItem.Quantity == 0)
            {
                updateOrCreateItem.Product = findProduct;
                updateOrCreateItem.Quantity = quantity;
                cart.Add(updateOrCreateItem);
            }
            else
            {
                updateOrCreateItem.Quantity += quantity;
            }

            // Ažuriraj sesiju
            HttpContext.Session.SetCartObjectAsJson(sessionCartKey, cart);

        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult RemoveFromCart(int productId)
    {
        // Pronađi sesiju i kreiraj varijablu generičke kolekcije
        List<CartItem> cart = HttpContext.Session.GetCartObjectFromJson(sessionCartKey);

        // Ukloni sve proizvode koji se podudaraju s Id-em parametra
        cart.RemoveAll(p => p.Product.Id == productId);

        // Ažuriraj sesiju
        HttpContext.Session.SetCartObjectAsJson(sessionCartKey, cart);

        return RedirectToAction(nameof(Index));

    }

    // GET: TestSession()
    public IActionResult TestSession()
    {
        // Primjer 1

        // Jednostavan primjer dodavanja sesije po ključu i vrijednosti
        HttpContext.Session.SetString("sessionString", "Ovo je moja vrijednost za string!");

        ViewBag.ReadSessionString = HttpContext.Session.GetString("sessionString");

        // Primjer ažuriranja vrijednosti postojećeg ključa sesije
        HttpContext.Session.SetString("sessionString", "Ja sam neki drugi text!");

        return View();
    }
}
