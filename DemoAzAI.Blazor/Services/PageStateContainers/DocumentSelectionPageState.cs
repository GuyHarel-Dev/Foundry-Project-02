namespace DemoAzAI.Blazor.Services.PageStateContainers
{
    public class DocumentSelectionPageState
    {
        public string DocFileNameSelected { get; set; } = string.Empty;
        public List<string> DocFileNames { get; set; } = new();

    }
}
