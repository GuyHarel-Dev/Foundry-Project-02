using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;

namespace DemoAzAI.Api.Services.Interfaces
{
    public interface IDocumentAnalysisService
    {
        Task<DocumentAnalysisResponseDto> AnalyzeDocumentAsync(DocumentAnalysisRequestDto doc, CancellationToken cancellationToken = default);
    }
}
