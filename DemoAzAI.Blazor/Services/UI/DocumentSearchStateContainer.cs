namespace DemoAzAI.Blazor.Services.UI
{
    public class DocumentSearchStateContainer : IDisposable
    {
        public string ExecutionLog { get; set; } = string.Empty;
        public string Statistics { get; set; } = string.Empty;
        public string DocContent { get; set; } = string.Empty;
        public string DocFields { get; set; } = string.Empty;

        public void Dispose()
        {
            
        }
    }
}
