using DemoAzAI.Abstractions.Entities;
using DemoAzAI.Domain.Models.DocumentAnalysis;

namespace DemoAzAI.Contracts.Api.Requests
{
    public class DocumentRequestDto
    {
        public DocumentEntity DocumentEntity { get; set; } = new();

        //public string FileName { get; set; } = string.Empty;
        //public string BytesHash { get; set; } = string.Empty;
        //public DocumentAnalysisResult? DocumentAnalysisResult { get; set; }
    }
}
