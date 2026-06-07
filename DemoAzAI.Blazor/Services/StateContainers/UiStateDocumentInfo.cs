namespace DemoAzAI.Blazor.Services.StateContainers
{
    public class UiStateDocumentInfo
    {
        public string Id { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public override string ToString()
        {
            return $"{Id}-{FileName}";
        }
    }
}
