using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces
{
    public interface IDocumentKnowledgeRepository
    {
        Task<DocumentChunkEntity?> GetDocumentKnowledgeAsync(string fileName);
        Task<bool> SaveDocumentChunkEntityAsync(DocumentChunkEntity document);
    }
}
