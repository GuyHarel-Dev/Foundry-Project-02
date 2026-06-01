using DemoAzAI.Abstractions;
using DemoAzAI.Domain;

namespace DemoAzAI.Infrastructure.ExternalServices.Stores
{
    public class DocumentAnalysisStoreService : IDocumentAnalysisStoreService
    {
        public Task<bool> ExistsAsync(string hash)
        {
            throw new NotImplementedException();
        }

        public Task<DocumentAnalysisResult?> GetByHashAsync(string hash)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(string hash, DocumentAnalysisResult result)
        {
            throw new NotImplementedException();
        }
    }
}
