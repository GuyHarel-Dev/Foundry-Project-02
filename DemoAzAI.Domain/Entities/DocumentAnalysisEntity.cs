using DemoAzAI.Domain.Models;

namespace DemoAzAI.Domain.Entities
{
    public class DocumentAnalysisEntity : DocumentEntity
    {
        public DocumentAnalysisResult? DocumentAnalysisResult { get; set; } = default!;

        public string DocumentAnalysisResultJson { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Id}-{FileName}";
        }


    }
}
