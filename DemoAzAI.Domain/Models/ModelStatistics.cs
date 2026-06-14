namespace DemoAzAI.Domain.Models
{
    public record ModelStatistics
    (
        string Model,
        int? InputTokens,
        int? OutputTokens,
        int? TotalTokens,
        int AnswerLength
    );
}
