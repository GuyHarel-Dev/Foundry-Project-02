using AzureAI.Contracts.Responses;

namespace AzureAI.Api.Services.Interfaces
{
    public interface IAzAiDocIntelligenceService
    {
        public Task<AnalyzeDocumentResponseDto> AnalyzeDocument();
    }
}
