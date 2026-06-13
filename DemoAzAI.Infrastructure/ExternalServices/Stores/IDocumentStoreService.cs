using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Infrastructure.ExternalServices.Stores
{
    public interface IDocumentStoreService
    {
        Task<List<string>> GetDocumentNames();

        Task<Stream> GetDocumentFileStreamAsync(string fileName);
    }
}
