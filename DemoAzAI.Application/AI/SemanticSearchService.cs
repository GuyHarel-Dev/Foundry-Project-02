using DemoAzAI.Domain;
using DemoAzAI.Infrastructure.ExternalServices.Stores;

namespace DemoAzAI.Application.AI
{
    public class SemanticSearchService : ISemanticSearchService
    {
        public List<SemanticSearchResult> Search(IEmbeddingStoreService embeddingStore, EmbeddedChunk embeddedQuestion)
        {
            var results = new List<SemanticSearchResult>();

            foreach (var embed in embeddingStore.Embeddings)
            {
                var score = VectorMath.CosineSimilarity(embeddedQuestion.EmbeddingVector, embed.EmbeddingVector);
                results.Add(new SemanticSearchResult
                {
                    Content = embed.Text,
                    Score = score
                });

            }

            return results;
        }
    }
}
