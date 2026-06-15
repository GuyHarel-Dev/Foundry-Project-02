using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;

namespace DemoAzAI.Api.Services.Interfaces
{
    public interface IDocumentAnalysisService
    {
        Task<DocumentAnalysisResponseDto> AnalyzeDocumentAsync(DocumentAnalysisRequestDto doc, CancellationToken cancellationToken = default);
        Task<IEnumerable<DocumentInfo>> GetAllDocumentAnalysisAsync();

        Task<DocumentAnalysisEntity?> GetDocumentAnalysisByNameAsync(string fileName);
        Task<DocumentAnalysisEntity?> GetDocumentAnalysisByIdAsync(string id);
    }
}
