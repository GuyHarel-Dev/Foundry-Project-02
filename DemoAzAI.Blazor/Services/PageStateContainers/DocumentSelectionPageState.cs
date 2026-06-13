namespace DemoAzAI.Blazor.Services.PageStateContainers
{
    public class DocumentSelectionPageState
    {
        public string? DocFileNameSelected { get; set; } = null;
        public IEnumerable<string> DocFileNames { get; set; } = default!;

    }
}
