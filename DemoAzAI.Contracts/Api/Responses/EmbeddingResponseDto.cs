using DemoAzAI.Domain;
using System.Text;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class EmbeddingResponseDto
    {
        public string RequestId { get; set; }
        //public List<EmbeddedChunkDto> TextEmbeddings { get; set; } = new List<EmbeddedChunkDto>();

        //public EmbeddingResponseDto(List<EmbeddedChunk> embeddedChunks)
        //{
        //    foreach(var chunk in embeddedChunks)
        //    {
        //        TextEmbeddings.Add(new EmbeddedChunkDto
        //        {
        //            Guid = chunk.Guid,
        //            Text = chunk.Text,
        //            EmbeddingVectorView = $"[{string.Join(", ", chunk.EmbeddingVector.Take(5))}...]"
        //        });
        //    }
        //}
        //public string GetSummary()
        //{
        //    var sb = new StringBuilder();
        //    sb.AppendLine($"Received {TextEmbeddings.Count} embeddings:");
        //    sb.AppendLine();

        //    foreach (var embedding in TextEmbeddings)
        //    {
        //        sb.AppendLine($"{embedding.Text}");
        //        sb.AppendLine($"[{string.Join(", ", embedding..Take(5))}...]");
        //        sb.AppendLine();

        //    }
        //    return sb.ToString();
        //}
    }
}
