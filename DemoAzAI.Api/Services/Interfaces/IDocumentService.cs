using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;

namespace DemoAzAI.Api.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<DocumentAnalysisEntity> AnalyzeDocumentAsync(string modelName, string fileName, bool saveToStore);
        Task<IEnumerable<DocumentInfo>> GetAllDocumentAnalysisAsync();

        Task<DocumentAnalysisEntity?> GetDocumentAnalysisByNameAsync(string fileName);
        Task<DocumentAnalysisEntity?> GetDocumentAnalysisByIdAsync(string id);
        Task<DocumentChunkEntity?> GetDocumentChunksByNameAsync(string fileName);
    }
}
