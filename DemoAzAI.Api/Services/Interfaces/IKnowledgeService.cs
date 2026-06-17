using DemoAzAI.Domain.Entities;

namespace WebApi.Services.Interfaces
{
    public interface IKnowledgeService
    {
        Task GetDocumentKnowledge(string documentId);
        Task<DocumentChunkEntity?> GetDocumentKnowledgeByFileNameAsync(string fileName);
    }
}
