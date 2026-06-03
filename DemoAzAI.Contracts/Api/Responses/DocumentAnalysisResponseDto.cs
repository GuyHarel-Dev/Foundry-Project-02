using DemoAzAI.Domain.Models.DocumentAnalysis;


namespace DemoAzAI.Contracts.Api.Responses
{
    public class DocumentAnalysisResponseDto
    {
        public string Status { get; set; } = string.Empty;
        public DocumentAnalysisResult? DocumentAnalysisResult { get; set; }

 
    }
}
