using DemoAzAI.Blazor.Services.StateContainers;

namespace DemoAzAI.Blazor.Services.StateContainers
{
    public class UiStateContainer
    {
        public UiStateDocumentSelection DocumentSelection { get; set; } = new();
        public UiStateDocumentAnalysis DocumentAnalysis { get; set; } = new();
        public UiStateSemanticAnalysis SemanticAnalysis { get; set; } = new();
    }
    

}
