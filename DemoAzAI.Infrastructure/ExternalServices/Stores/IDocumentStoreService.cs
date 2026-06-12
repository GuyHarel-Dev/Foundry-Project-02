using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Infrastructure.ExternalServices.Stores
{
    public interface IDocumentStoreService
    {
        Task<List<string>> GetDocumentNames();

        //Task<Stream> GetDocumentFile(StoreDocument document);
    }
}
