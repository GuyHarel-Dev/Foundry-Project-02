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
        public async Task<DocumentKnowledgeResponseDto?> EmbedDocumentKnowledgeAsync(DocumentKnowledgeRequestDto request)
        {
            var response = new DocumentKnowledgeResponseDto();

            var chunks = await ChunkDocumentTextAsync(request, response);

            if (chunks != null && chunks.Length > 0)
            {
                var embeddingResponse = await EmbedDocumentChunksAsync(request, chunks); 

                if (embeddingResponse != null && request.SaveToStore)
                {
                    await DeleteDocumentIfExists(request);

                    var docToSave = new DocumentKnowledgeEntity
                    {
                        FileName = request.FileName,
                        EmbeddedTexts = embeddingResponse.EmbeddedTexts
                    };

                    var saved = await documentKnowledgeRepository.SaveDocumentKnowledgeAsync(docToSave);
                }
            }

            return response;

        }

        private async Task DeleteDocumentIfExists(DocumentKnowledgeRequestDto request)
        {
            var exists = await documentKnowledgeRepository.DocumentKnowledgeExistsAsync(request.FileName);

            if (exists)
                await documentKnowledgeRepository.DeleteDocumentKnowledgeAsync(request.FileName);
        }

        private async Task<string[]?> ChunkDocumentTextAsync(DocumentKnowledgeRequestDto request, DocumentKnowledgeResponseDto response)
        {
            var chatRequest = new ChatQuestionRequestDto
            {
                Prompt = request.TextToChunk,
                SystemPrompt = AiPrompts.DocumentKnowledgeExtraction
            };

            var promptResponse = await chatService.GetPromptResponseAsync(chatRequest);

            if (promptResponse != null && promptResponse.ChatResponse != null)
            {
                response.ChunkingStatistics = promptResponse.ModelStatistics;
                return promptResponse.ChatResponse.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            }

            return [];
        }

        private async Task<EmbeddingTextLinesResponseDto?> EmbedDocumentChunksAsync(DocumentKnowledgeRequestDto request, string[] chunks)
        {
            if (request != null && chunks.Length > 0)
            {
                var embeddingRequest = new EmbeddingTextLinesRequestDto
                {
                    TextLines = chunks.ToList()
                };

                return  await embeddingService.EmbedTextLinesAsync(embeddingRequest);

            }

            return null;
        }

        public async Task<DocumentKnowledgeEntity?> GetDocumentKnowledgeByFileNameAsync(string fileName)
        {
            return await documentKnowledgeRepository.GetDocumentKnowledgeAsync(fileName);
        }

        public async Task<IEnumerable<DocumentInfo>?> GetDocumentKnowledgeEmbeddingsAsync()
        {
            return await documentKnowledgeRepository.GetDocumentKnowledgeEmbeddingsAsync();
        }
    }
}
