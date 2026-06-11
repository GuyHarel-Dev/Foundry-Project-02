using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories
{
    public interface IDocumentRepository
    {
        Task<bool> SaveAsync(DocumentEntity document);
        Task<List<DocumentEntity>> GetAllDocuments();
    }
}
