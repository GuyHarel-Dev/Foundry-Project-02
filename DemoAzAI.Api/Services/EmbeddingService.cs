using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using OpenAI.Embeddings;
using System.ClientModel;
using WebApi.Services.Interfaces;

#pragma warning disable OPENAI001 // pour using OpenAI.Responses;


namespace DemoAzAI.Api.Services
{
    public class EmbeddingService(
            EmbeddingClient embeddingClient,
            ILogger<EmbeddingService> logger)
        : IEmbeddingService

    {
        public async Task<EmbeddingTextResponseDto> EmbedTextAsync(string text, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Starting embedding generation for text: {text}", text);

            ClientResult<OpenAIEmbedding> result = await embeddingClient
                .GenerateEmbeddingAsync(text, cancellationToken: cancellationToken);

            var embeddedText = new DocumentEmbeddingEntity
            {
                ChunkText = text,
                EmbeddingVector = result.Value.ToFloats().ToArray()
            };
            
            return new EmbeddingTextResponseDto
            {
                EmbeddedText = embeddedText 
            };
        }

        public async Task<EmbeddingTextLinesResponseDto> EmbedTextLinesAsync(EmbeddingTextLinesRequestDto request, CancellationToken cancellationToken = default)
        {
            var textLines = request.TextLines;

            logger.LogInformation("Starting embedding generation for {Count} text lines.", textLines.Count);

            ClientResult<OpenAIEmbeddingCollection> result = await embeddingClient
                .GenerateEmbeddingsAsync(textLines, cancellationToken: cancellationToken);

            var response = new EmbeddingTextLinesResponseDto
            {
                EmbeddedTexts = new List<DocumentEmbeddingEntity>()
            };

            foreach (var emb in result.Value)
            {
                response.EmbeddedTexts.Add(new DocumentEmbeddingEntity 
                    { 
                        ChunkText = textLines.ElementAt(emb.Index), 
                        EmbeddingVector = emb.ToFloats().ToArray() 
                });
            }

            return response;


        }

    }
}

