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
            IDocumentKnowledgeRepository documentChunksRepository
        ) : IKnowledgeService
    {
        public async Task<DocumentKnowledgeResponseDto> GetDocumentKnowledgeAsync(DocumentKnowledgeRequestDto request)
        {

            var chunks = await ChunkDocumentTextAsync(request);

            if (chunks != null && chunks.Length > 0)
            {

                var response = await EmbedDocumentChunksAsync(request, chunks);

                if (response != null && request.SaveToStore)
                {
                    var status = SaveEmbeddedTextLinesToStore(response);
                }
            }


            return new DocumentKnowledgeResponseDto();

        }

        private async Task<bool> SaveEmbeddedTextLinesToStore(EmbeddingResponseDto response)
        {

            var status = await documentChunksRepository.SaveDocumentChunkEntityAsync(response.DocumentChunkEntity);

            return true;
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
                    var entity = new DocumentChunkEntity
                    {
                        FileName = request.FileName,
                        EmbeddedChunks = new List<EmbeddedChunk>()
                    };

                    entity.EmbeddedChunks.AddRange(response.DocumentChunkEntity.EmbeddedChunks);

                    return new EmbeddingResponseDto
                    {
                        DocumentChunkEntity = entity
  
                    };
                }
            }


            return null;
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
