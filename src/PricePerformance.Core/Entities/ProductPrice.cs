namespace PricePerformance.Core.Entities;

public class ProductPrice : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public int PlatformId { get; set; }
    public Platform Platform { get; set; } = null!;
    
    public int CountryId { get; set; }
    public Country Country { get; set; } = null!;
    
    public decimal Price { get; set; }
    public decimal? OriginalPrice { get; set; }
    public bool IsInStock { get; set; } = true;
    public string ProductUrl { get; set; } = string.Empty;
    public DateTime PriceDate { get; set; } = DateTime.UtcNow;
}
