using DemoAzAI.Domain.Models.DocumentAnalysis;

namespace DemoAzAI.Abstractions
{
    public interface IDocumentAnalysisStoreService
    {
        Task<IEnumerable<DocumentAnalysisHistoryItem>> GetAllAsync();

        Task<DocumentAnalysisResult?> GetByHashAsync(string hash);

        Task SaveAsync(string hash, DocumentAnalysisResult result);

        Task<bool> ExistsAsync(string hash);
    }
}
