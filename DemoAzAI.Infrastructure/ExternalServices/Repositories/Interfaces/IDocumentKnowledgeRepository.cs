using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces
{
    public interface IDocumentKnowledgeRepository
    {
        Task<DocumentChunkEntity?> GetDocumentsync(string fileName);
        Task<bool> SaveDocumentAsync(DocumentChunkEntity document);

        Task<bool> DocumentExistsAsync(string fileName);
        Task<bool> DeleteDocumentAsync(string fileName);
    }
}
