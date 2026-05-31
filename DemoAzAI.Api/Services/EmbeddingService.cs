using Azure.AI.Projects;
using DemoAzAI.AI;
using DemoAzAI.Application.AI;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain;
using DemoAzAI.Infrastructure.ExternalServices.Stores;
using OpenAI.Embeddings;
using System.ClientModel;
using WebApi.Services.Interfaces;

#pragma warning disable OPENAI001 // pour using OpenAI.Responses;

namespace DemoAzAI.Api.Services
{
    public class EmbeddingService(
            EmbeddingClient embeddingClient,
            ILogger<EmbeddingService> logger,
            IEmbeddingStoreService embeddingStoreService,
            ISemanticSearchService semanticSearchService)
        : IEmbeddingService

    {
        public int EmbeddingStoreCount { get; } = embeddingStoreService.Count;

        public async Task<List<SemanticSearchResult>?> SearchEmbedding(string? question)
        {
            logger.LogInformation($"Starting embedding search for question: {question}");

            if (question == null) 
                throw new ArgumentNullException(nameof(question));

            // Embed the question first
            var request = new EmbeddingRequestDto
            {
                AddEmbeddingsToStore = false,
                Chunks = new List<string> { question }
            };

            var response = await EmbeddTextAsync(request);

            //// Retrieve the embedding vector for the question
            //var embeddedQuestion = response.TextEmbeddings.First();

            //if (embeddedQuestion is null)  return null;

            // Search the stored embeddings for relevant chunks
            return new List<SemanticSearchResult>();  //semanticSearchService.Search(embeddingStoreService, embeddedQuestion);


        }

        public async Task<EmbeddingResponseDto> EmbeddTextAsync(EmbeddingRequestDto request, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Starting embedding generation for {Count} text lines.", request.Chunks.Count);

            var chunkArray = request.Chunks.ToArray();

            ClientResult<OpenAIEmbeddingCollection> result = await embeddingClient
                .GenerateEmbeddingsAsync(chunkArray, cancellationToken: cancellationToken);

            EmbeddingResponseDto response = new();

            foreach (OpenAIEmbedding embedding in result.Value)
            {
                EmbeddedChunk embeddedChunk = new EmbeddedChunk(
                    Guid.NewGuid(),
                    chunkArray[embedding.Index],
                    embedding.ToFloats().ToArray()
                );

                response.TextEmbeddings.Add(embeddedChunk);
            }

            return response;

        }

        public void StoreEmbeddings(EmbeddingRequestDto request, OpenAIEmbeddingCollection embeddings)
        {
            var storedEmbeddings = new List<EmbeddedChunk>();
            foreach (var embedding in embeddings)
            {
                var storedEmbedding = new EmbeddedChunk(
                    Guid.NewGuid(),
                    request.Chunks[embedding.Index],
                    embedding.ToFloats().ToArray()
                );
                storedEmbeddings.Add(storedEmbedding);
            }
            AddNewEmbeddingsToStore(storedEmbeddings);
        }

        //public async Task<EmbeddingResponseDto> EmbeddTextAsync(EmbeddingRequestDto request, CancellationToken cancellationToken = default)
        //{
        //    logger.LogInformation("Starting embedding generation for {Count} text lines.", request.Chunks.Count);

        //    OpenAIEmbeddingCollection result = await embeddingClient.GenerateEmbeddingsAsync(request.Chunks.ToArray(), cancellationToken: cancellationToken);

        //    var storedEmbeddings = new List<EmbeddedChunk>();

        //    foreach (var embedding in result)
        //    {
        //        var storedEmbedding = new EmbeddedChunk(
        //            Guid.NewGuid(),
        //            request.Chunks[embedding.Index],
        //            embedding.ToFloats().ToArray()
        //        );

        //        storedEmbeddings.Add(storedEmbedding);

        //    }

        //    if (request.AddEmbeddingsToStore)
        //        AddNewEmbeddingsToStore(storedEmbeddings);

        //    return new EmbeddingResponseDto(storedEmbeddings);

        //}

        private void AddNewEmbeddingsToStore(IEnumerable<EmbeddedChunk> newEmbeddings)
        {
            foreach (var embedding in newEmbeddings)
            {
                if (embeddingStoreService.Embeddings.Contains(embedding))
                {
                    logger.LogInformation($"Embedding for text '{embedding.Text}' already exists in the store. Skipping.");
                    continue;
                }
                embeddingStoreService.AddEmbedding(embedding);
            }
        }
    }
}

