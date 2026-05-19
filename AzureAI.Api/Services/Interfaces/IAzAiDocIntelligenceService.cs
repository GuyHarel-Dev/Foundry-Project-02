using AzureAI.Contracts.Requests;
using AzureAI.Contracts.Responses;

namespace AzureAI.Api.Services.Interfaces
{
    public interface IAzAiDocIntelligenceService
    {
        Task<AnalyzeDocumentResponseDto> AnalyzeDocument(AnalyzeDocumentRequestDto doc, CancellationToken cancellationToken = default);
    }
}
