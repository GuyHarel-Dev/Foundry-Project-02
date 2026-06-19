using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces
{
    public interface IDocumentKnowledgeRepository
    {
        Task<bool> SaveDocumentKnowledgeAsync(DocumentEmbeddingEntity document);
        Task<DocumentEmbeddingEntity?> GetDocumentKnowledgeAsync(string fileName);
        Task<bool> DocumentKnowledgeExistsAsync(string fileName);
        Task<bool> DeleteDocumentKnowledgeAsync(string fileName);
    }
}
