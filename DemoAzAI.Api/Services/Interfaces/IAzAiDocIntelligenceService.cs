using DemoAzAI.Contracts.Requests;
using DemoAzAI.Contracts.Responses;

namespace DemoAzAI.Api.Services.Interfaces
{
    public interface IAzAiDocIntelligenceService
    {
        Task<AnalyzeDocumentResponseDto> AnalyzeDocument(AnalyzeDocumentRequestDto doc, CancellationToken cancellationToken = default);
    }
}
