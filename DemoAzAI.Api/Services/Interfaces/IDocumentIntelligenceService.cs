using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;

namespace DemoAzAI.Api.Services.Interfaces
{
    public interface IDocumentIntelligenceService
    {
        Task<DocumentAnalysisResponseDto> AnalyzeDocument(DocumenteAnalysisRequestDto doc, CancellationToken cancellationToken = default);
    }
}
