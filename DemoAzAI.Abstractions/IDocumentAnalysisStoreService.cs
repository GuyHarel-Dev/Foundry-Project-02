using DemoAzAI.Domain;

namespace DemoAzAI.Abstractions
{
    public interface IDocumentAnalysisStoreService
    {
        Task<DocumentAnalysisResult?> GetByHashAsync(string hash);

        Task SaveAsync(string hash, DocumentAnalysisResult result);

        Task<bool> ExistsAsync(string hash);
    }
}
