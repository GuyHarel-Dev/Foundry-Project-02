using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;

namespace WebApi.Services.Interfaces
{
    public interface IKnowledgeService
    {
        Task<DocumentKnowledgeResponseDto> EmbedDocumentKnowledgeAsync(DocumentKnowledgeRequestDto request);
        Task<DocumentKnowledgeEntity?> GetDocumentKnowledgeByFileNameAsync(string fileName);
        Task<IEnumerable<DocumentInfo>?> GetDocumentKnowledgeEmbeddingsAsync();


    }
}
