using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
using DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class KnowledgeService(
            IChatService chatService,
            IEmbeddingService embeddingService,
            IDocumentKnowledgeRepository documentKnowledgeRepository
        ) : IKnowledgeService
    {
        public async Task<DocumentKnowledgeResponseDto> EmbedDocumentKnowledgeAsync(DocumentKnowledgeRequestDto request)
        {

            var chunks = await ChunkDocumentTextAsync(request);

            if (chunks != null && chunks.Length > 0)
            {

                var response = await EmbedDocumentChunksAsync(request, chunks); 

                if (response != null && request.SaveToStore)
                {
                    var exists = await documentKnowledgeRepository.DocumentExistsAsync(request.FileName);

                    if (exists)
                        await documentKnowledgeRepository.DeleteDocumentAsync(request.FileName);

                    var status = await documentKnowledgeRepository.SaveDocumentAsync(response.DocumentEmbeddingEntity);
                }
            }


            return new DocumentKnowledgeResponseDto();

        }

        private async Task<string[]?> ChunkDocumentTextAsync(DocumentKnowledgeRequestDto request)
        {
            var chatRequest = new ChatQuestionRequestDto
            {
                Prompt = request.TextToChunk,
                SystemPrompt = request.SystemPrompt
            };

            var response = await chatService.GetPromptResponseAsync(chatRequest);

            if (response != null && response.ChatResponse != null)
            {
                return response.ChatResponse.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            }

            return [];
        }

        private async Task<EmbeddingResponseDto?> EmbedDocumentChunksAsync(DocumentKnowledgeRequestDto request, string[] chunks)
        {
            if (request != null && chunks.Length > 0)
            {
                var embeddingRequest = new EmbeddingTextLinesRequestDto
                {
                    TextLines = chunks.ToList()
                };

                var response = await embeddingService.EmbedTextLinesAsync(embeddingRequest);

                if (response != null)
                {
                    var entity = new DocumentEmbeddingEntity
                    {
                        FileName = request.FileName,
                        EmbeddedChunks = new List<EmbeddedChunk>()
                    };

                    entity.EmbeddedChunks.AddRange(response.DocumentEmbeddingEntity.EmbeddedChunks);

                    return new EmbeddingResponseDto
                    {
                        DocumentEmbeddingEntity = entity
  
                    };
                }
            }


            return null;
        }

        public async Task<DocumentEmbeddingEntity?> GetDocumentChunksByNameAsync(string fileName)
        {
            // return await documentChunksRepository.GetDocumentChunksAsync(fileName);
            return new DocumentEmbeddingEntity();
        }

        public Task GetDocumentKnowledge(string documentId)
        {
            throw new NotImplementedException();
        }

        public Task<DocumentEmbeddingEntity?> GetDocumentKnowledgeByFileNameAsync(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
