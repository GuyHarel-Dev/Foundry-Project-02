using AzureAI.Contracts.Responses;

namespace AzureAI.Api.Services.Interfaces
{
    public interface IAzAiDocIntelligenceService
    {
        Task<AnalyzeDocumentResponseDto> AnalyzeDocument(string model, Stream stream, CancellationToken cancellationToken = default);
    }
}
