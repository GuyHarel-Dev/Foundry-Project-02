using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain;
using OpenAI.Embeddings;
using System.ClientModel;
using WebApi.Services.Interfaces;

#pragma warning disable OPENAI001 // pour using OpenAI.Responses;

namespace DemoAzAI.Api.Services
{
    public class EmbeddingService(
            EmbeddingClient embeddingClient,
            ILogger<EmbeddingService> logger)
            //IEmbeddingStoreService embeddingStoreService,
            //ISemanticSearchServicej semanticSearchService)
        : IEmbeddingService

    {
        //public int EmbeddingStoreCount { get; } = embeddingStoreService.Count;

        //public async Task<List<SemanticSearchResult>> SearchEmbeddingStoreForSimilarTextLinesAsync(string? question)
        //{
        //    var embeddedQuestion = await EmbeddTextAsync(question ?? string.Empty);

        //    var result = semanticSearchService.GetSemanticalySimilarEmbeddings(embeddingStoreService, embeddedQuestion);

        //    return result;
        //}


        public async Task<EmbeddedChunk> EmbedTextAsync(string text, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Starting embedding generation for text: {text}", text);

            ClientResult<OpenAIEmbedding> result = await embeddingClient
                .GenerateEmbeddingAsync(text, cancellationToken: cancellationToken);

            return new EmbeddedChunk
            (
                Guid.NewGuid(),
                text,
                result.Value.ToFloats().ToArray()
            );
        }

        public async Task<EmbeddingResponseDto> EmbedTextLinesAsync(EmbeddingRequestDto request, CancellationToken cancellationToken = default)
        {
            var textLines = request.TextLines.ToArray();

            logger.LogInformation("Starting embedding generation for {Count} text lines.", textLines.Length);

            ClientResult<OpenAIEmbeddingCollection> result = await embeddingClient
                .GenerateEmbeddingsAsync(textLines, cancellationToken: cancellationToken);

            EmbeddingResponseDto response = new();

            foreach (OpenAIEmbedding embedding in result.Value)
            {
                EmbeddedChunk embeddedChunk = new EmbeddedChunk(
                    Guid.NewGuid(),
                    textLines[embedding.Index],
                    embedding.ToFloats().ToArray()
                );

                response.TextEmbeddings.Add(embeddedChunk);
            }

            if (request.AddEmbeddingsToStore)
                AddNewEmbeddingsToStore(response.TextEmbeddings);

            return response;

        }

        public void StoreEmbeddings(EmbeddingRequestDto request, OpenAIEmbeddingCollection embeddings)
        {
            var storedEmbeddings = new List<EmbeddedChunk>();
            foreach (var embedding in embeddings)
            {
                var storedEmbedding = new EmbeddedChunk(
                    Guid.NewGuid(),
                    request.TextLines[embedding.Index],
                    embedding.ToFloats().ToArray()
                );
                storedEmbeddings.Add(storedEmbedding);
            }
            AddNewEmbeddingsToStore(storedEmbeddings);
        }

        private void AddNewEmbeddingsToStore(IEnumerable<EmbeddedChunk> newEmbeddings)
        {
            foreach (var embedding in newEmbeddings)
            {
                //if (embeddingStoreService.Embeddings.Contains(embedding))
                //{
                //    logger.LogInformation($"Embedding for text '{embedding.Text}' already exists in the store. Skipping.");
                //    continue;
                //}
                //embeddingStoreService.AddEmbedding(embedding);
            }
        }

 
    }
}

