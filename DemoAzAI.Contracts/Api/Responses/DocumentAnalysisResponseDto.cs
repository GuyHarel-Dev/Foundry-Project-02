using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class DocumentAnalysisResponseDto
    {
        public DocumentAnalysisEntity? DocumentAnalysisEntity { get; set; }

        public string DocumentAnalysisStatus { get; set; } = string.Empty;
 
    }
}
