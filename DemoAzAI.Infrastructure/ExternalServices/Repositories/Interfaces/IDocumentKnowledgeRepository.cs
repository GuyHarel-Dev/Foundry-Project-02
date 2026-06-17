using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces
{
    public interface IDocumentKnowledgeRepository
    {
        Task<bool> SaveAsync(DocumentChunkEntity document);
        Task<DocumentChunkEntity?> GetDocumentKnowledgeAsync(string fileName);
    }
}
