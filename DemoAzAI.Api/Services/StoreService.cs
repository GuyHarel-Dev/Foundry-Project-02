using DemoAzAI.Abstractions.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class StoreService : IStoreService
    {
        public Task<IEnumerable<DocumentEntity>> GetAllDocuments()
        {
            throw new NotImplementedException();
        }
    }
}
