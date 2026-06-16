using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;

namespace DemoAzAI.Blazor.Services
{
    public interface IDocumentService
    {
        Task<DocumentAnalysisResponseDto?> GetDocumentAnalysisByNameAsync(string fileName, ILogger? logger, List<string>? pageLogger);
        Task<IEnumerable<DocumentInfo>?> GetAllDocumentInfosFromStoreAsync(ILogger? logger = null, List<string>? errorMessages = null);
    }
}
