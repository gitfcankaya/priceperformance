using PricePerformance.Core.Interfaces;

namespace PricePerformance.Infrastructure.Services;

public class AIContentService : IAIContentService
{
    // This is a placeholder implementation
    // In production, this would integrate with OpenAI and Gemini APIs
    
    public async Task<string> GenerateOriginalDescriptionAsync(string originalText)
    {
        // Placeholder - would call OpenAI/Gemini API here
        await Task.Delay(100);
        return $"AI-Generated: {originalText}";
    }

    public async Task<string> GenerateOriginalReviewAsync(string originalText)
    {
        // Placeholder - would call OpenAI/Gemini API here
        await Task.Delay(100);
        return $"AI-Generated Review: {originalText}";
    }
}
