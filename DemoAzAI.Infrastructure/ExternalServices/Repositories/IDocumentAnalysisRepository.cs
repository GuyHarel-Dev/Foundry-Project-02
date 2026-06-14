using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories
{
    public interface IDocumentAnalysisRepository
    {
        Task<IEnumerable<DocumentAnalysisInfo>> GetDocumentAnalysisAsync();
        Task<DocumentAnalysisEntity?> GetDocumentAnalysisByIdAsync(string id);
        Task<DocumentAnalysisEntity?> GetDocumentAnalysisByHashAsync(string hash);

        Task<bool> SaveDocumentAnalysisAsync(string fileName, DocumentAnalysisResult? result, string BytesHash);

        Task<bool> DocumentAnalysisExistsAsync(string hash);
    }
}
