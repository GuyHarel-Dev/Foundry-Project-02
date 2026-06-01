using DemoAzAI.Domain;
using DemoAzAI.Abstractions;

namespace DemoAzAI.Application.AI
{
    public class SemanticSearchService : ISemanticSearchService
    {
        public List<SemanticSearchResult> GetSemanticalySimilarEmbeddings(IEmbeddingStoreService embeddingStore, EmbeddedChunk? embeddedQuestion)
        {
            var results = new List<SemanticSearchResult>();

            if (embeddedQuestion is null)
                return results;

            //foreach (var embed in embeddingStore.Embeddings)
            //{
            //    var score = VectorMath.CosineSimilarity(embeddedQuestion.EmbeddingVector, embed.EmbeddingVector);
            //    results.Add(new SemanticSearchResult
            //    {
            //        Content = embed.Text,
            //        Score = score
            //    });

            //}

            return results;
        }
    }
}
