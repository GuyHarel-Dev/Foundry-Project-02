using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Application.AI;
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
                    var exists = await documentKnowledgeRepository.DocumentKnowledgeExistsAsync(request.FileName);

                    if (exists)
                        await documentKnowledgeRepository.DeleteDocumentKnowledgeAsync(request.FileName);

                    //var status = await documentKnowledgeRepository.SaveDocumentKnowledgeAsync(response.DocumentEmbeddingEntity);
                }
            }


            return new DocumentKnowledgeResponseDto();

        }

        private async Task<string[]?> ChunkDocumentTextAsync(DocumentKnowledgeRequestDto request)
        {
            var chatRequest = new ChatQuestionRequestDto
            {
                Prompt = request.TextToChunk,
                SystemPrompt = AiPrompts.DocumentKnowledgeExtraction
            };

            var response = await chatService.GetPromptResponseAsync(chatRequest);

            if (response != null && response.ChatResponse != null)
            {
                return response.ChatResponse.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            }

            return [];
        }

        private async Task<EmbeddingTextResponseDto?> EmbedDocumentChunksAsync(DocumentKnowledgeRequestDto request, string[] chunks)
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
                    //var entity = new DocumentEmbeddingEntity
                    //{
                    //    FileName = request.FileName,
                    //    EmbeddedChunks = new List<EmbeddedChunk>()
                    //};

                    //entity.EmbeddedChunks.AddRange(response.DocumentEmbeddingEntity.EmbeddedChunks);

                    //return new EmbeddingResponseDto
                    //{
                    //    DocumentEmbeddingEntity = entity
  
                    //};
                }
            }


            return null;
        }

        public async Task<DocumentEmbeddingEntity?> GetDocumentKnowledgeByFileNameAsync(string fileName)
        {
            var test = await documentKnowledgeRepository.GetDocumentKnowledgeAsync(fileName);
            return new DocumentEmbeddingEntity();
        }
    }
}
