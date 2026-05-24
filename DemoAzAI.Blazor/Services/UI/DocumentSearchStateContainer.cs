namespace DemoAzAI.Blazor.Services.UI
{
    public class DocumentSearchStateContainer : IDisposable
    {
        public string ExecutionLog { get; set; } = string.Empty;
        public string Statistics { get; set; } = string.Empty;
        public string DocContent { get; set; } = string.Empty;
        public string DocFields { get; set; } = string.Empty;
        public string PdfUrl { get; set; } = string.Empty;


        public void SetPdf(string url)
        {
            PdfUrl = url;
        }

        public void Reset()
        {
            ExecutionLog = string.Empty;
            Statistics = string.Empty;
            DocContent = string.Empty;
            DocFields = string.Empty;
            PdfUrl = string.Empty;
        }

        public void Dispose()
        {
            Reset();
        }
    }
}
