using DemoAzAI.Blazor.Services.StateContainers;
using DemoAzAI.Domain.Models.DocumentAnalysis;

namespace DemoAzAI.Blazor.Services.UI
{
    public class UiStateContainer
    {
        public UiStateDocumentSelection DocumentSelection { get; set; } = new UiStateDocumentSelection();
        public UiStateDocumentAnalysis DocumentAnalysis { get; set; } = new UiStateDocumentAnalysis();
    }
    

}
