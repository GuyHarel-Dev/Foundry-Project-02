using System.Text;

namespace DemoAzAI.Domain.Entities
{
    public class DocumentKnowledgeEntity
    {
        public string? DocumentId { get; set; }
        public string? FileName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<DocumentEmbeddingEntity>? Embeddings { get; set; }

        public string CosineSimilaritiesToString(float threshold)
        {
            var sb = new StringBuilder();

            if (Embeddings != null)
            {
                foreach (var emb in Embeddings)
                {
                    sb.AppendLine($"{emb.CosineSimilarity} [{emb.ChunkText}]");
                }
            }

            return sb.ToString();
        }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"DocumentId:{DocumentId}");
            sb.AppendLine($"FileName:{FileName}");
            
            if ( Embeddings != null )
            {
                foreach (var emb in Embeddings)
                { 
                    sb.AppendLine($"  {emb.ChunkText}, ");
                    sb.AppendLine($"  [{emb.EmbeddingVectorJson?.Substring(1, 128)}...]");
                    sb.AppendLine();
                }
            }
           
            return sb.ToString();
        }
    }
}
 