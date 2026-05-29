using DemoAzAI.Domain;
using DemoAzAI.Infrastructure.ExternalServices.Stores;

namespace DemoAzAI.AI
{
    public class SemanticSearchService : ISemanticSearchService
    {
        public void Search(IEmbeddingStoreService embeddingStore, string question)
        {
            var test = 123;
        }
    }
}
