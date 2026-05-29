using DemoAzAI.Api.Contracts.Requests;
using DemoAzAI.Api.Contracts.Responses;

namespace DemoAzAI.Api.Services.Interfaces
{
    public interface IDocumentIntelligenceService
    {
        Task<AnalyzeDocumentResponseDto> AnalyzeDocument(AnalyzeDocumentRequestDto doc, CancellationToken cancellationToken = default);
    }
}
