using DemoAzAI.Abstractions.Entities;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class RepositoryService : IStoreService
    {
        public Task<IEnumerable<DocumentEntity>> GetAllDocuments()
        {
            throw new NotImplementedException();
        }
    }
}
