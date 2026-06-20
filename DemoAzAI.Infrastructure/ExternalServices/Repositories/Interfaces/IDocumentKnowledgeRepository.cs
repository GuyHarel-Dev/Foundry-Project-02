using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces
{
    public interface IDocumentKnowledgeRepository
    {
        Task<bool> SaveDocumentKnowledgeAsync(DocumentKnowledgeEntity document);
        Task<DocumentKnowledgeEntity?> GetDocumentKnowledgeAsync(string fileName);
        Task<IEnumerable<DocumentInfo>?> GetDocumentKnowledgeEmbeddingsAsync();
        Task<bool> DocumentKnowledgeExistsAsync(string fileName);
        Task<bool> DeleteDocumentKnowledgeAsync(string fileName);
    }
}
