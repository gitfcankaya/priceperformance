namespace PricePerformance.Core.Entities;

public class ProductReview : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public string ReviewerName { get; set; } = string.Empty;
    public string ReviewText { get; set; } = string.Empty;
    public string? OriginalReviewText { get; set; }
    public int Rating { get; set; }
    public DateTime ReviewDate { get; set; }
    public string? Platform { get; set; }
    public bool IsVerified { get; set; } = false;
}
