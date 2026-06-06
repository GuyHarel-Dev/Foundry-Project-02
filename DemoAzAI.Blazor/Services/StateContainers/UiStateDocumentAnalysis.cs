using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Models.DocumentAnalysis;

namespace DemoAzAI.Blazor.Services.StateContainers
{
    public class UiStateDocumentAnalysis
    {
        public DocumentAnalysisResponseDto? DocumentAnalysis { get; set; }
    }
}
