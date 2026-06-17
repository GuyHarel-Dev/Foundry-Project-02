using DemoAzAI.Domain.Entities;
using DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class KnowledgeService(
            IDocumentKnowledgeRepository documentChunksRepository
        ) : IKnowledgeService
    {
        public Task<string?> GetDocumentKnowledgeByNameAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task<DocumentChunkEntity?> GetDocumentChunksByNameAsync(string fileName)
        {
            // return await documentChunksRepository.GetDocumentChunksAsync(fileName);
            return new DocumentChunkEntity();
        }

        public Task GetDocumentKnowledge(string documentId)
        {
            throw new NotImplementedException();
        }

        public Task<DocumentChunkEntity?> GetDocumentKnowledgeByFileNameAsync(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
