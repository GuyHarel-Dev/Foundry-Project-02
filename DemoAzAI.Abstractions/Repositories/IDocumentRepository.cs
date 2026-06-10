using DemoAzAI.Abstractions.Entities;

namespace DemoAzAI.Abstractions.Repositories
{
    public interface IDocumentRepository
    {
        Task<bool> SaveAsync(DocumentEntity document);
        Task<List<DocumentEntity>> GetAllDocuments();
    }
}
