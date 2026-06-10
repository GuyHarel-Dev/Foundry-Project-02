using DemoAzAI.Abstractions.Entities;

namespace WebApi.Services.Interfaces
{
    public interface IStoreService
    {
        Task<IEnumerable<DocumentEntity>> GetAllDocuments();
    }
}
