using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;

namespace WebApi.Services.Interfaces
{
    public interface IKnowledgeService
    {
        Task<DocumentKnowledgeResponseDto> EmbedDocumentKnowledgeAsync(DocumentKnowledgeRequestDto request);
        Task<DocumentEmbeddingEntity?> GetDocumentKnowledgeByFileNameAsync(string fileName);
    }
}
