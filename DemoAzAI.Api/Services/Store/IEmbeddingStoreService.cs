namespace WebApi.Services.Store
{
    public interface IEmbeddingStoreService
    {
        List<StoredEmbedding> Embeddings { get; set; }
        void AddEmbedding(StoredEmbedding storedEmbedding);
        void AddEmbeddings(IEnumerable<StoredEmbedding> storedEmbedding);
        void ClearEmbeddings();
        StoredEmbedding? GetEmbedding(Guid id);
    }
}
