using DemoAzAI.Domain.Models.DocumentAnalysis;


namespace DemoAzAI.Contracts.Api.Responses
{
    public class DocumentAnalysisResponseDto
    {
        public string FileName { get; set; } = string.Empty;
        public string BytesHash { get; set; } = string.Empty;
        public DocumentAnalysisResult? DocumentAnalysisResult { get; set; }

 
    }
}
