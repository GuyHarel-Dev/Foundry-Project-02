using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Infrastructure.ExternalServices.Stores
{
    public interface IFileStoreService
    {
        Task<List<StoreDocument>> GetAllDocuments();
    }
}
