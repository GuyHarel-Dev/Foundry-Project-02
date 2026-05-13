using Contracts.Responses;

namespace WebApi.Services.Interfaces
{
    public interface IAzAiDocIntelligenceService
    {
        public Task<AnalyzeDocumentResponseDto> AnalyzeDocument();
    }
}
