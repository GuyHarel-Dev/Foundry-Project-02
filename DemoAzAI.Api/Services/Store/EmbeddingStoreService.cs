using DemoAzAI.Contracts.Dtos;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace WebApi.Services.Store
{
    public class EmbeddingStoreService : IEmbeddingStoreService
    {
        public List<TextEmbeddingDto> Embeddings { get; set; } = new List<TextEmbeddingDto>();
        IReadOnlyCollection<TextEmbeddingDto> IEmbeddingStoreService.Embeddings { get => Embeddings; set => throw new NotImplementedException(); }

        public void AddEmbeddings(IEnumerable<TextEmbeddingDto> storedEmbedding)
        {
            Embeddings.AddRange(storedEmbedding);
        }

        public void AddEmbedding(TextEmbeddingDto storedEmbedding)
        {
            Embeddings.Add(storedEmbedding);
        }

        public void ClearEmbeddings()
        {
            Embeddings.Clear();
        }

        TextEmbeddingDto? IEmbeddingStoreService.GetEmbedding(Guid id)
        {
            return Embeddings.FirstOrDefault(e => e.Guid == id);
        }
    }
}
