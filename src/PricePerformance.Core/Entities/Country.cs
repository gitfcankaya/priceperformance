namespace PricePerformance.Core.Entities;

public class Country : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string CurrencySymbol { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    public ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();
}
