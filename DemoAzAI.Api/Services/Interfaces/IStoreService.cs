using DemoAzAI.Domain;

namespace WebApi.Services.Interfaces
{
    public interface IStoreService
    {
        void StoreDocumentAnalysisResult(DocumentAnalysisResult doc);
        
    }
}
