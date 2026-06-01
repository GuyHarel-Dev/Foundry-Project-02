using DemoAzAI.Abstractions;
using DemoAzAI.Domain;

namespace DemoAzAI.Application.AI
{
    public interface ISemanticSearchService
    {
        List<SemanticSearchResult> GetSemanticalySimilarEmbeddings(IEmbeddingStoreService embeddingStore, EmbeddedChunk embeddedQuestion);
    }
}
