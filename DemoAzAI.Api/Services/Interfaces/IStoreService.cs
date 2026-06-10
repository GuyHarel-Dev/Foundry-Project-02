using DemoAzAI.Abstractions.Models;

namespace WebApi.Services.Interfaces
{
    public interface IStoreService
    {
        Task<IEnumerable<DocumentEntity>> GetAllDocuments();
    }
}
