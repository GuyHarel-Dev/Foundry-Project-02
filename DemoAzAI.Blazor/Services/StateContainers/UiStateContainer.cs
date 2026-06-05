namespace DemoAzAI.Blazor.Services.UI
{
    public class UiStateContainer : IDisposable
    {
        public string DocumentSelected { get; set; } = "(empty) Select a document in page Document Selection";

        public void Reset()
        {
            DocumentSelected = string.Empty;
        }

        public void Dispose()
        {
            Reset();
        }
    }
}
