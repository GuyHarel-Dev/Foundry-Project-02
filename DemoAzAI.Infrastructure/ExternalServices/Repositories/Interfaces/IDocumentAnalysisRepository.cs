using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces
{
    public interface IDocumentAnalysisRepository
    {
        Task<IEnumerable<DocumentInfo>> GetAllDocumentAnalysisAsync();
        Task<DocumentAnalysisEntity?> GetDocumentAnalysisByNameAsync(string fileName);
        Task<DocumentAnalysisEntity?> GetDocumentAnalysisByIdAsync(string id);
        Task<DocumentAnalysisEntity?> GetDocumentAnalysisByHashAsync(string hash);

        Task<bool> SaveDocumentAnalysisAsync(string fileName, DocumentAnalysisResult? result, string BytesHash);

        Task<bool> DocumentAnalysisExistsAsync(string hash);
    }
}
