using DemoWebShop.Models;
using DemoWebShop.Services.Cart;
using DemoWebShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebshop.Controllers;

public class OrderController : Controller
{
    // Ključ naše sesije za košaricu
    public const string sessionCartKey = "_cart";


    // GET: Order/Checkout
    public IActionResult Checkout()
    {
        // Korak 1: pronađi sesiju i provjeri ako postoji barem jedan proizvod u košarici
        List<CartItem> cart = HttpContext.Session.GetCartObjectFromJson(sessionCartKey);

        if (cart.Count <= 0)
        {
            return RedirectToAction("Index", "Home");
        }

        // Korak 2: definiraj ViewBag za ispis poruka
        ViewBag.CheckoutMessages = TempData["CheckoutMessages"] as string ?? "";

        return View(cart);
    }

    //TODO: CreateOrder(Order newOrder)
    [HttpPost]
    public IActionResult CreateOrder(Order newOrder)
    {
        // 1. korak - Provjera ako je košarica prazna!
        // 2. korak - Provjeri ako je model klase validan (required i ostala polja)
        // 3. korak - Pohrana u bazu, čišćenje koašrice, preusmjeravanje, itd...

        List<CartItem> cart = HttpContext.Session.GetCartObjectFromJson(sessionCartKey);
        if (cart.Count <= 0)
        {
            return RedirectToAction("Index", "Home");
        }

        var modelErrors = new List<string>();
        if (ModelState.IsValid)
        {
            // true - sva svojstva su validna
        }
        else
        {
            // false - neki podatak nije validan!
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    modelErrors.Add(error.ErrorMessage);
                }
            }

            /*
             * Primjer:
             * Error email,
             * Error first name,
             * Error last name, ...
             * 
             * Retultat: Error email <br /> Error first name <br /> Error last name <br /> ...
             */
            TempData["CheckoutMessages"] = String.Join("<br />", modelErrors);
            return RedirectToAction("Checkout");
        }




        return View();
    }
}
