using DemoAzAI.Abstractions;
using DemoAzAI.Domain;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class StoreService (
        IDocumentAnalysisStoreService documentAnalysisStoreService
        )  : IStoreService
    {
        public void StoreDocumentAnalysisResult(DocumentAnalysisResult doc)
        {
            var hash = new HashString(doc).ToString();
            documentAnalysisStoreService.SaveAsync(hash, doc);
        }
    }
}
