using Microsoft.EntityFrameworkCore;
using PricePerformance.Core.Entities;

namespace PricePerformance.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Seed Countries
        if (!await context.Countries.AnyAsync())
        {
            var countries = new List<Country>
            {
                new() { Name = "United States", Code = "US", Currency = "USD", CurrencySymbol = "$" },
                new() { Name = "United Kingdom", Code = "GB", Currency = "GBP", CurrencySymbol = "£" },
                new() { Name = "Germany", Code = "DE", Currency = "EUR", CurrencySymbol = "€" },
                new() { Name = "Turkey", Code = "TR", Currency = "TRY", CurrencySymbol = "₺" },
                new() { Name = "France", Code = "FR", Currency = "EUR", CurrencySymbol = "€" },
                new() { Name = "Spain", Code = "ES", Currency = "EUR", CurrencySymbol = "€" },
                new() { Name = "Italy", Code = "IT", Currency = "EUR", CurrencySymbol = "€" },
                new() { Name = "Japan", Code = "JP", Currency = "JPY", CurrencySymbol = "¥" },
                new() { Name = "Canada", Code = "CA", Currency = "CAD", CurrencySymbol = "C$" },
                new() { Name = "Australia", Code = "AU", Currency = "AUD", CurrencySymbol = "A$" }
            };
            await context.Countries.AddRangeAsync(countries);
        }

        // Seed Platforms
        if (!await context.Platforms.AnyAsync())
        {
            var platforms = new List<Platform>
            {
                new() { Name = "Amazon", BaseUrl = "https://www.amazon.com", LogoUrl = "/images/platforms/amazon.png" },
                new() { Name = "eBay", BaseUrl = "https://www.ebay.com", LogoUrl = "/images/platforms/ebay.png" },
                new() { Name = "AliExpress", BaseUrl = "https://www.aliexpress.com", LogoUrl = "/images/platforms/aliexpress.png" },
                new() { Name = "Walmart", BaseUrl = "https://www.walmart.com", LogoUrl = "/images/platforms/walmart.png" },
                new() { Name = "Target", BaseUrl = "https://www.target.com", LogoUrl = "/images/platforms/target.png" },
                new() { Name = "Best Buy", BaseUrl = "https://www.bestbuy.com", LogoUrl = "/images/platforms/bestbuy.png" },
                new() { Name = "Trendyol", BaseUrl = "https://www.trendyol.com", LogoUrl = "/images/platforms/trendyol.png" },
                new() { Name = "Hepsiburada", BaseUrl = "https://www.hepsiburada.com", LogoUrl = "/images/platforms/hepsiburada.png" },
                new() { Name = "N11", BaseUrl = "https://www.n11.com", LogoUrl = "/images/platforms/n11.png" }
            };
            await context.Platforms.AddRangeAsync(platforms);
        }

        // Seed Categories
        if (!await context.Categories.AnyAsync())
        {
            var categories = new List<Category>
            {
                new() { Name = "Electronics", Slug = "electronics", Description = "Electronic devices and gadgets" },
                new() { Name = "Computers & Laptops", Slug = "computers-laptops", Description = "Computers, laptops and accessories", ParentCategoryId = 1 },
                new() { Name = "Smartphones & Tablets", Slug = "smartphones-tablets", Description = "Mobile phones and tablets", ParentCategoryId = 1 },
                new() { Name = "Home & Garden", Slug = "home-garden", Description = "Home and garden products" },
                new() { Name = "Fashion", Slug = "fashion", Description = "Clothing and accessories" },
                new() { Name = "Men's Clothing", Slug = "mens-clothing", Description = "Men's fashion items", ParentCategoryId = 5 },
                new() { Name = "Women's Clothing", Slug = "womens-clothing", Description = "Women's fashion items", ParentCategoryId = 5 },
                new() { Name = "Sports & Outdoors", Slug = "sports-outdoors", Description = "Sports equipment and outdoor gear" },
                new() { Name = "Books & Media", Slug = "books-media", Description = "Books, movies, music" },
                new() { Name = "Toys & Games", Slug = "toys-games", Description = "Toys and games for all ages" }
            };
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        // Seed Products
        if (!await context.Products.AnyAsync())
        {
            var electronicsCategory = await context.Categories.FirstAsync(c => c.Slug == "electronics");
            var computersCategory = await context.Categories.FirstAsync(c => c.Slug == "computers-laptops");
            var smartphonesCategory = await context.Categories.FirstAsync(c => c.Slug == "smartphones-tablets");

            var products = new List<Product>
            {
                new()
                {
                    Name = "Apple iPhone 15 Pro 256GB",
                    Slug = "apple-iphone-15-pro-256gb",
                    Description = "Latest iPhone with A17 Pro chip, titanium design, and advanced camera system",
                    Brand = "Apple",
                    Model = "iPhone 15 Pro",
                    CategoryId = smartphonesCategory.Id,
                    ImageUrl = "/images/products/iphone-15-pro.jpg"
                },
                new()
                {
                    Name = "Samsung Galaxy S24 Ultra 512GB",
                    Slug = "samsung-galaxy-s24-ultra-512gb",
                    Description = "Premium Android smartphone with S Pen, 200MP camera, and AI features",
                    Brand = "Samsung",
                    Model = "Galaxy S24 Ultra",
                    CategoryId = smartphonesCategory.Id,
                    ImageUrl = "/images/products/galaxy-s24-ultra.jpg"
                },
                new()
                {
                    Name = "MacBook Pro 14-inch M3 Pro",
                    Slug = "macbook-pro-14-m3-pro",
                    Description = "Powerful laptop with M3 Pro chip, Liquid Retina XDR display",
                    Brand = "Apple",
                    Model = "MacBook Pro 14",
                    CategoryId = computersCategory.Id,
                    ImageUrl = "/images/products/macbook-pro-14.jpg"
                },
                new()
                {
                    Name = "Dell XPS 15 9530",
                    Slug = "dell-xps-15-9530",
                    Description = "High-performance laptop with Intel Core i9, NVIDIA RTX 4070",
                    Brand = "Dell",
                    Model = "XPS 15",
                    CategoryId = computersCategory.Id,
                    ImageUrl = "/images/products/dell-xps-15.jpg"
                },
                new()
                {
                    Name = "Sony WH-1000XM5 Wireless Headphones",
                    Slug = "sony-wh-1000xm5",
                    Description = "Industry-leading noise canceling headphones with premium sound quality",
                    Brand = "Sony",
                    Model = "WH-1000XM5",
                    CategoryId = electronicsCategory.Id,
                    ImageUrl = "/images/products/sony-wh1000xm5.jpg"
                }
            };
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            // Seed Product Prices
            var amazon = await context.Platforms.FirstAsync(p => p.Name == "Amazon");
            var bestBuy = await context.Platforms.FirstAsync(p => p.Name == "Best Buy");
            var usa = await context.Countries.FirstAsync(c => c.Code == "US");
            var turkey = await context.Countries.FirstAsync(c => c.Code == "TR");

            var iphone = await context.Products.FirstAsync(p => p.Slug == "apple-iphone-15-pro-256gb");
            var samsung = await context.Products.FirstAsync(p => p.Slug == "samsung-galaxy-s24-ultra-512gb");

            var prices = new List<ProductPrice>
            {
                new()
                {
                    ProductId = iphone.Id,
                    PlatformId = amazon.Id,
                    CountryId = usa.Id,
                    Price = 1099.00M,
                    OriginalPrice = 1199.00M,
                    IsInStock = true,
                    ProductUrl = "https://www.amazon.com/iphone-15-pro",
                    PriceDate = DateTime.UtcNow
                },
                new()
                {
                    ProductId = iphone.Id,
                    PlatformId = bestBuy.Id,
                    CountryId = usa.Id,
                    Price = 1149.00M,
                    IsInStock = true,
                    ProductUrl = "https://www.bestbuy.com/iphone-15-pro",
                    PriceDate = DateTime.UtcNow
                },
                new()
                {
                    ProductId = samsung.Id,
                    PlatformId = amazon.Id,
                    CountryId = usa.Id,
                    Price = 1299.99M,
                    OriginalPrice = 1419.99M,
                    IsInStock = true,
                    ProductUrl = "https://www.amazon.com/samsung-s24-ultra",
                    PriceDate = DateTime.UtcNow
                }
            };
            await context.ProductPrices.AddRangeAsync(prices);
        }

        await context.SaveChangesAsync();
    }
}
