using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories
{
    public interface IDocumentRepository
    {
        Task<bool> SaveAsync(StoreDocument document);
        Task<List<StoreDocument>> GetAllDocuments();
    }
}
