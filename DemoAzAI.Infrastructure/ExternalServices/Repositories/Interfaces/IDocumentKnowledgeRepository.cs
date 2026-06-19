using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces
{
    public interface IDocumentKnowledgeRepository
    {
        Task<DocumentEmbeddingEntity?> GetDocumentsync(string fileName);
        Task<bool> SaveDocumentAsync(DocumentEmbeddingEntity document);
        Task<DocumentEmbeddingEntity?> GetDocumentAsync(DocumentEmbeddingEntity document);
        Task<bool> DocumentExistsAsync(string fileName);
        Task<bool> DeleteDocumentAsync(string fileName);
    }
}
