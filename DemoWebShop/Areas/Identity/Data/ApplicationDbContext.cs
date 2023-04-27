using DemoWebShop.Areas.Identity.Data;
using DemoWebShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Policy;

namespace DemoWebshop.Data;


public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{

    // Mapiraj C# klase modela s tablicama u bazi podataka
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        // Seeding za Kategorije
        List<Category> mainCategories = new List<Category>()
        {
            new Category() { Id = 1, Title = "Mliječni proizvodi" },
            new Category() { Id = 2, Title = "Domaći proizvodi" },
            new Category() { Id = 3, Title = "Proteinska hrana" },
            new Category() { Id = 4, Title = "Hrana za kućne ljubimce" },
            new Category() { Id = 5, Title = "Nakit" },

        };
        
        builder.Entity<Category>().HasData(mainCategories);
 


        // Postavke za seedanje uloga (eng.: roles) i glavnog administratora

        // Tablica AspNetRoles - Identity klasa IdentityRole
        string adminRoleId = "6217999e-a9fb-448b-b163-e2305fc44f50";
        string adminRoleTitle = "Admin";
        string customerRoleId = "0e71d461-63e3-4aa5-be93-d701a5a1f913";
        string customerRoleTitle = "Customer";

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole()
            {
                Id = adminRoleId,
                Name = adminRoleTitle,
                NormalizedName = adminRoleTitle.ToUpper()
            },
            new IdentityRole()
            {
                Id = customerRoleId,
                Name = customerRoleTitle,
                NormalizedName = customerRoleTitle.ToUpper()
            }
        );

        // Tablica AspNetUsers - Identity klasa ApplicationUser (izvorna: IdentityUser)
        string adminId = "66412151-dd0c-4b69-82c8-0f4256e78f00";
        string admin = "mico@admin.com"; /// i korisničko ime i email vrijednost
        string adminFirstName = "Mićo";
        string adminLastName = "Programerić";
        string adminPassword = "secret";
        string adminAddress = "Stara Cesta bb";

        // Za Hash lozinke
        var hasher = new PasswordHasher<ApplicationUser>();

        builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser()
            {
                Id = adminId,
                UserName = admin,
                NormalizedUserName = admin.ToUpper(),
                Email = admin,
                NormalizedEmail = admin.ToUpper(),
                FirstName = adminFirstName,
                LastName = adminLastName,
                Address = adminAddress,
                PasswordHash = hasher.HashPassword(null, adminPassword)
            }
        );
        // Tablica AspNetUserRoles - Identity klasa IdentityUserRole<string> (veza između Users i Roles)
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>()
            {
                UserId = adminId,
                RoleId = adminRoleId
            }
        );

        List<Product> mainProducts = new List<Product>()
        {
            new Product() { Id = 101, Title = "Yogurtos", Description = "High in protein", Sku = "S006", InStock = 30, Price = 6.90M},
            new Product() { Id = 102, Title = "Almie", Description = "Halves", Sku = "S009", InStock = 40, Price = 3.90M},
            new Product() { Id = 103, Title = "Light", Description = "wink wink", Sku = "S206", InStock = 50, Price = 1.90M},
            new Product() { Id = 104, Title = "Meat", Description = "Diet", Sku = "S906", InStock = 20, Price = 22.90M},
            new Product() { Id = 105, Title = "Rings", Description = "Chick and classy look", Sku = "S216", InStock = 50, Price = 70.90M}
        };
        builder.Entity<Product>().HasData(mainProducts);

    }
}
