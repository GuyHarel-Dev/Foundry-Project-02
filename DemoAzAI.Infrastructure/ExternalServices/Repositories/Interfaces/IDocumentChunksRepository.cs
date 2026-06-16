using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces
{
    public interface IDocumentChunksRepository
    {
        Task<bool> SaveAsync(DocumentChunkEntity document);
        Task<List<DocumentChunkEntity>> GetDocumentChunksAsync(string fileName);
    }
}
