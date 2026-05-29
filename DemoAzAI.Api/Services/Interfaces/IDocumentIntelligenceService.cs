using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;

namespace DemoAzAI.Api.Services.Interfaces
{
    public interface IDocumentIntelligenceService
    {
        Task<AnalyzeDocumentResponseDto> AnalyzeDocument(AnalyzeDocumentRequestDto doc, CancellationToken cancellationToken = default);
    }
}
