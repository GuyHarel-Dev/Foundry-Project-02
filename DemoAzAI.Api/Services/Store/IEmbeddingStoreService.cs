namespace WebApi.Services.Store
{
    public interface IEmbeddingStoreService
    {
        void AddEmbedding(StoredEmbedding storedEmbedding);
        void AddEmbeddings(IEnumerable<StoredEmbedding> storedEmbedding);
        void ClearEmbeddings();
        StoredEmbedding? GetEmbedding(Guid id);
    }
}
