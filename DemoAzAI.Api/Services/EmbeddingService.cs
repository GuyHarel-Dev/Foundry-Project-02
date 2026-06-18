using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
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

            var chunk = new EmbeddedChunk
            (
                Guid.NewGuid(),
                text,
                result.Value.ToFloats().ToArray()
            );

            return new EmbeddingResponseDto { EmbeddedChunks = new List<EmbeddedChunk> { chunk } };
        }

        public async Task<EmbeddingResponseDto> EmbedTextLinesAsync(EmbeddingTextLinesRequestDto request, CancellationToken cancellationToken = default)
        {
            var textLines = request.TextLines;

            logger.LogInformation("Starting embedding generation for {Count} text lines.", textLines.Count);

            ClientResult<OpenAIEmbeddingCollection> result = await embeddingClient
                .GenerateEmbeddingsAsync(request.TextLines, cancellationToken: cancellationToken);

            EmbeddingResponseDto response = new();

            foreach (OpenAIEmbedding embedding in result.Value)
            {
                EmbeddedChunk embeddedChunk = new EmbeddedChunk(
                    Guid.NewGuid(),
                    textLines[embedding.Index],
                    embedding.ToFloats().ToArray()
                );

                response.EmbeddedChunks.Add(embeddedChunk);
            }


            return response;

        }
 
    }
}

