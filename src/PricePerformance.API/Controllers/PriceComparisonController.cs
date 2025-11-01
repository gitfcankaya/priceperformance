using Microsoft.AspNetCore.Mvc;
using PricePerformance.Core.Interfaces;

namespace PricePerformance.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PriceComparisonController : ControllerBase
{
    private readonly IPriceComparisonService _priceComparisonService;

    public PriceComparisonController(IPriceComparisonService priceComparisonService)
    {
        _priceComparisonService = priceComparisonService;
    }

    [HttpGet("best-price/{productId}")]
    public async Task<ActionResult<BestPriceResult>> GetBestPrice(int productId)
    {
        var result = await _priceComparisonService.GetBestPriceAsync(productId);
        if (result == null)
            return NotFound("No prices found for this product");

        return Ok(result);
    }

    [HttpGet("best-prices-category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<BestPriceResult>>> GetBestPricesForCategory(int categoryId)
    {
        var results = await _priceComparisonService.GetBestPricesForCategoryAsync(categoryId);
        return Ok(results);
    }

    [HttpGet("price-history/{productId}")]
    public async Task<ActionResult> GetPriceHistory(int productId, [FromQuery] DateTime? startDate)
    {
        var history = await _priceComparisonService.GetPriceHistoryAsync(productId, startDate);
        return Ok(history);
    }
}
