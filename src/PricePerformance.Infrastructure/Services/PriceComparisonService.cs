using Microsoft.EntityFrameworkCore;
using PricePerformance.Core.Entities;
using PricePerformance.Core.Interfaces;
using PricePerformance.Infrastructure.Data;

namespace PricePerformance.Infrastructure.Services;

public class PriceComparisonService : IPriceComparisonService
{
    private readonly ApplicationDbContext _context;

    public PriceComparisonService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BestPriceResult?> GetBestPriceAsync(int productId)
    {
        var product = await _context.Products
            .Include(p => p.ProductPrices)
                .ThenInclude(pp => pp.Platform)
            .Include(p => p.ProductPrices)
                .ThenInclude(pp => pp.Country)
            .FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null || !product.ProductPrices.Any())
            return null;

        var latestPrices = product.ProductPrices
            .Where(pp => pp.IsInStock)
            .GroupBy(pp => new { pp.PlatformId, pp.CountryId })
            .Select(g => g.OrderByDescending(pp => pp.PriceDate).First())
            .ToList();

        if (!latestPrices.Any())
            return null;

        var bestPrice = latestPrices.OrderBy(pp => pp.Price).First();
        var maxPrice = latestPrices.Max(pp => pp.Price);

        return new BestPriceResult
        {
            Product = product,
            BestPrice = bestPrice,
            SavingsAmount = maxPrice - bestPrice.Price,
            SavingsPercentage = maxPrice > 0 ? ((maxPrice - bestPrice.Price) / maxPrice) * 100 : 0
        };
    }

    public async Task<IEnumerable<BestPriceResult>> GetBestPricesForCategoryAsync(int categoryId)
    {
        var products = await _context.Products
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .Include(p => p.ProductPrices)
                .ThenInclude(pp => pp.Platform)
            .Include(p => p.ProductPrices)
                .ThenInclude(pp => pp.Country)
            .ToListAsync();

        var results = new List<BestPriceResult>();

        foreach (var product in products)
        {
            var latestPrices = product.ProductPrices
                .Where(pp => pp.IsInStock)
                .GroupBy(pp => new { pp.PlatformId, pp.CountryId })
                .Select(g => g.OrderByDescending(pp => pp.PriceDate).First())
                .ToList();

            if (latestPrices.Any())
            {
                var bestPrice = latestPrices.OrderBy(pp => pp.Price).First();
                var maxPrice = latestPrices.Max(pp => pp.Price);

                results.Add(new BestPriceResult
                {
                    Product = product,
                    BestPrice = bestPrice,
                    SavingsAmount = maxPrice - bestPrice.Price,
                    SavingsPercentage = maxPrice > 0 ? ((maxPrice - bestPrice.Price) / maxPrice) * 100 : 0
                });
            }
        }

        return results.OrderByDescending(r => r.SavingsPercentage);
    }

    public async Task<IEnumerable<ProductPrice>> GetPriceHistoryAsync(int productId, DateTime? startDate = null)
    {
        var query = _context.ProductPrices
            .Include(pp => pp.Platform)
            .Include(pp => pp.Country)
            .Where(pp => pp.ProductId == productId);

        if (startDate.HasValue)
        {
            query = query.Where(pp => pp.PriceDate >= startDate.Value);
        }

        return await query
            .OrderBy(pp => pp.PriceDate)
            .ToListAsync();
    }
}
