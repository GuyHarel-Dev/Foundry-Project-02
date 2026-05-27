using DemoAzAI.Contracts.Dtos;

namespace WebApi.Services.Store
{
    public interface IEmbeddingStoreService
    {
        List<TextEmbeddingDto> Embeddings { get; set; }
        void AddEmbedding(TextEmbeddingDto storedEmbedding);
        void AddEmbeddings(IEnumerable<TextEmbeddingDto> storedEmbedding);
        void ClearEmbeddings();
        TextEmbeddingDto? GetEmbedding(Guid id);
    }
}
