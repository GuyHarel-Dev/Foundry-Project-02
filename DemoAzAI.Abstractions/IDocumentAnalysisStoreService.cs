using DemoAzAI.Domain.Models.DocumentAnalysis;

namespace DemoAzAI.Abstractions
{
    public interface IDocumentAnalysisStoreService
    {
        Task<IEnumerable<DocumentAnalysisHistoryItem>> GetAllAsync();

        Task<DocumentAnalysisResult?> GetByHashAsync(string hash);

        Task<bool> SaveAsync(string fileName, string documentHash, DocumentAnalysisResult result);

        Task<bool> ExistsAsync(string hash);
    }
}
