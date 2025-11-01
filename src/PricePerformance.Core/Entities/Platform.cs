namespace PricePerformance.Core.Entities;

public class Platform : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    public ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();
}
