using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;

namespace WebApi.Services.Interfaces
{
    public interface IKnowledgeService
    {
        Task<DocumentKnowledgeResponseDto> GetDocumentKnowledgeAsync(DocumentKnowledgeRequestDto request);
        Task<DocumentChunkEntity?> GetDocumentKnowledgeByFileNameAsync(string fileName);
    }
}
