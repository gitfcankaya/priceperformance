using PricePerformance.Core.Entities;

namespace PricePerformance.Core.Interfaces;

public class BestPriceResult
{
    public Product Product { get; set; } = null!;
    public ProductPrice BestPrice { get; set; } = null!;
    public decimal SavingsAmount { get; set; }
    public decimal SavingsPercentage { get; set; }
}

public interface IPriceComparisonService
{
    Task<BestPriceResult?> GetBestPriceAsync(int productId);
    Task<IEnumerable<BestPriceResult>> GetBestPricesForCategoryAsync(int categoryId);
    Task<IEnumerable<ProductPrice>> GetPriceHistoryAsync(int productId, DateTime? startDate = null);
}
