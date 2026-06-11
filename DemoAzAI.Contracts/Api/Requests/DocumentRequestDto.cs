using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Contracts.Api.Requests
{
    public class DocumentRequestDto
    {
        public StoreDocument DocumentEntity { get; set; } = new();

        //public string FileName { get; set; } = string.Empty;
        //public string BytesHash { get; set; } = string.Empty;
        //public DocumentAnalysisResult? DocumentAnalysisResult { get; set; }
    }
}
