using PricePerformance.Core.Entities;

namespace PricePerformance.Core.Interfaces;

public interface ICrawlerService
{
    Task<IEnumerable<Product>> CrawlProductsAsync(string platformName, string category);
    Task<IEnumerable<ProductPrice>> CrawlPricesAsync(int productId);
}
