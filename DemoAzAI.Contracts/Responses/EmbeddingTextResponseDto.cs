using DemoAzAI.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DemoAzAI.Contracts.Responses
{
    public class EmbeddingTextResponseDto
    {
        public List<TextEmbeddingDto> TextEmbeddings { get; set; } = new List<TextEmbeddingDto>();
        public string GetSummary()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Received {TextEmbeddings.Count} embeddings:");

            foreach (var embedding in TextEmbeddings)
            {
                sb.AppendLine($"[{string.Join(", ", embedding.Vector.Take(5))}...] --> {embedding.Text}");

            }
            return sb.ToString();
        }
    }
}
