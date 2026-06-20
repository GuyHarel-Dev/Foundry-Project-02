using DemoAzAI.Domain.Entities;
using System.Text;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class EmbeddingTextLinesResponseDto
    {
        public List<DocumentEmbeddingEntity>? EmbeddedTexts { get; set; }

        public string GetSummary()
        {
            var sb = new StringBuilder();
            //foreach (var embedding in DocumentEmbeddingEntity.EmbeddedChunks)
            //{
            //    sb.AppendLine($"{embedding.ChunkText}: [{string.Join(", ", embedding.EmbeddingVector.Take(5))}...]");

            //}
            return sb.ToString();
        }
    }
}
