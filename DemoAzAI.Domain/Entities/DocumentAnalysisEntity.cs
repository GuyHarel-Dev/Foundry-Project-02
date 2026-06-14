using DemoAzAI.Domain.Models;

namespace DemoAzAI.Domain.Entities
{
    public class DocumentAnalysisEntity
    {
        public string Id { get; set; } = string.Empty;

        public string Hash { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string AnalysisResultJson { get; set; } = string.Empty;
        public DocumentAnalysisResult AnalysisResult { get; set; } = default!;

        public override string ToString()
        {
            return $"{Id}-{FileName}";
        }


    }
}
