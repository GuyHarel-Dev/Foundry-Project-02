using System.Text;

namespace DemoAzAI.Domain.Models.DocumentAnalysis
{
    public class DocumentAnalysisStoredResult
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

        public string ToText()
        {
            var sb = new StringBuilder();

            sb.AppendLine(AnalysisResult.DocContent);

            foreach (var doc in AnalysisResult.Documents)
            {
                foreach (var field in doc.DocumentFields)
                {
                    sb.AppendLine(field.Name + ": " + field.Value);
                }
            }
            return sb.ToString();
        }
    }
}
