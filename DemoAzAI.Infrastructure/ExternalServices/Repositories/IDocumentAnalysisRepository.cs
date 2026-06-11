using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models.DocumentAnalysis;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories
{
    public interface IDocumentAnalysisRepository
    {
        Task<IEnumerable<DocumentAnalysisEntity>> GetAllAsync();
        Task<DocumentAnalysisEntity?> GetByIdAsync(string id);
        Task<DocumentAnalysisEntity?> GetByHashAsync(string hash);

        Task<bool> SaveAsync(string fileName, string documentHash, DocumentAnalysisResult result);

        Task<bool> ExistsAsync(string hash);
    }
}
