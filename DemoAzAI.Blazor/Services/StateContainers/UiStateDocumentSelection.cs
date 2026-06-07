namespace DemoAzAI.Blazor.Services.StateContainers
{
    public class UiStateDocumentSelection
    {
        public List<UiStateDocumentInfo> AvailableDocuments { get; set; } = new();
        public UiStateDocumentInfo? SelectedDocument { get; set; } = null;

    }
}
