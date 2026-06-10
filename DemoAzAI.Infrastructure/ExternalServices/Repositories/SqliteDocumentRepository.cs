using DemoAzAI.Abstractions.Models;
using DemoAzAI.Abstractions.Repositories;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories
{
    internal class SqliteDocumentRepository : IDocumentRepository
    {
        public List<DocumentEntity> GetAllDocuments()
        {
            throw new NotImplementedException();
        }
    }
}
