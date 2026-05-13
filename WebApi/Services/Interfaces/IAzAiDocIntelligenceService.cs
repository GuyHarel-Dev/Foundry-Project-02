using Shared.DTOs;

namespace WebApi.Services.Interfaces
{
    public interface IAzAiDocIntelligenceService
    {
        public Task<AnalyzeDocumentResponseDto> AnalyzeDocument();
    }
}
