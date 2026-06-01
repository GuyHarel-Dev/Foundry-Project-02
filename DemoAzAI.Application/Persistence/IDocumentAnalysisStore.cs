using DemoAzAI.Domain;

namespace DemoAzAI.Application.Persistence
{
    public interface IDocumentAnalysisStore
    {
        Task<DocumentAnalysisResult?> GetByHashAsync(string hash);
        Task SaveAsync(DocumentAnalysisResult record);
    }
}
