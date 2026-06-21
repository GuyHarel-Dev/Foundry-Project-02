using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Api.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<DocumentAnalysisEntity> AnalyzeDocumentAsync(string modelName, string fileName, bool saveToStore);

    }
}
