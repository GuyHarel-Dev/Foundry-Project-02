using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class DocumentAnalysisResponseDto
    {
        public DocumentAnalysisEntity? DocumentAnalysisEntity { get; set; }

        public string DocumentAnalysisStatus { get; set; } = string.Empty;
 
    }
}
