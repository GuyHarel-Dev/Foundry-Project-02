using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;

namespace DemoAzAI.Api.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<DocumentAnalysisEntity> AnalyzeDocumentAsync(string modelName, string fileName, bool saveToStore);

    }
}
