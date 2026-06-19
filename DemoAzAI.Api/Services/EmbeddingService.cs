using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
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
        public async Task<EmbeddingResponseDto> EmbedTextAsync(string text, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Starting embedding generation for text: {text}", text);

            ClientResult<OpenAIEmbedding> result = await embeddingClient
                .GenerateEmbeddingAsync(text, cancellationToken: cancellationToken);

            var index = 0;
            var chunk = new EmbeddedChunk
            (
                index,
                text,
                result.Value.ToFloats().ToArray()
            );

            return new EmbeddingResponseDto
            {
                DocumentEmbeddingEntity = new DocumentEmbeddingEntity
                {
                    EmbeddedChunks = new List<EmbeddedChunk> { chunk }
                }
            };
        }

        public async Task<EmbeddingResponseDto> EmbedTextLinesAsync(EmbeddingTextLinesRequestDto request, CancellationToken cancellationToken = default)
        {
            var textLines = request.TextLines;

            logger.LogInformation("Starting embedding generation for {Count} text lines.", textLines.Count);

            ClientResult<OpenAIEmbeddingCollection> result = await embeddingClient
                .GenerateEmbeddingsAsync(request.TextLines, cancellationToken: cancellationToken);

            EmbeddingResponseDto response = new();

            var index = 0;
            foreach (OpenAIEmbedding embedding in result.Value)
            {
                EmbeddedChunk embeddedChunk = new EmbeddedChunk(
                    index++,
                    textLines[embedding.Index],
                    embedding.ToFloats().ToArray()
                );

                response.DocumentEmbeddingEntity.EmbeddedChunks.Add(embeddedChunk);
            }


            return response;

        }

    }
}

