using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Infrastructure.ExternalServices.Stores
{
    public interface IFileStoreService
    {
        Task<List<string>> GetAllDocumentNames();

        //Task<Stream> GetDocumentFile(StoreDocument document);
    }
}
