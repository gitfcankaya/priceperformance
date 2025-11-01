namespace PricePerformance.Core.Interfaces;

public interface IAIContentService
{
    Task<string> GenerateOriginalDescriptionAsync(string originalText);
    Task<string> GenerateOriginalReviewAsync(string originalText);
}
