using DemoAzAI.Infrastructure.ExternalServices.Stores;

namespace DemoAzAI.AI
{
    public interface ISemanticSearchService
    {
        void Search(IEmbeddingStoreService embeddingStore, string question);
    }
}
