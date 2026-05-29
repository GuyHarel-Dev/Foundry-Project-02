using DemoAzAI.Domain;
using DemoAzAI.Infrastructure.ExternalServices.Stores;

namespace DemoAzAI.Application.AI
{
    public interface ISemanticSearchService
    {
        List<SemanticSearchResult> Search(IEmbeddingStoreService embeddingStore, EmbeddedChunk embeddedQuestion);
    }
}
