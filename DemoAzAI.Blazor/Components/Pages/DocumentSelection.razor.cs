using DemoAzAI.Blazor.Services.Api;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public partial class DocumentSelection
    {
        [Inject]
        public ApiClient apiClient { get; set; } = default!;

        [Inject]
        private ILogger<DocumentSelection> logger { get; set; } = default!;

        [Inject]
        private IConfiguration configuration { get; set; }

        private void OnDocumentSelected(ChangeEventArgs e)
        {
            var docFileName = e.Value?.ToString();

            if (string.IsNullOrEmpty(docFileName)) return;

            if (docFileNames is null) return;

            docFileNameSelected = docFileNames.FirstOrDefault(name => name == docFileName);

        }

        private string DocFileNameUrl ()
        {
            var baseUrl = configuration["FoundryWebApi:BaseUrl"] ?? "https://localhost:5001"; // Default URL if not specified in configuration


            return $"{baseUrl}/api/documents/content/empty-pdf.pdf";
        }

        private async Task Test()
        {
            //var result = await ApiClientGetAsync<Stream>("api/documents/content/empty-pdf.pdf", logger);

            //var result = await apiClient.GetAsync<File>("api/documents/content/empty-pdf.pdf");


            //fileStream = result.Value;

        }

        private async Task LoadDocumentNamesAsync()
        {
            try
            {
                loadDocumentIsBusy = true;

                var result = await apiClient.GetAsync<IEnumerable<string>>("api/documents/names", logger);

                if (result is not null)
                {
                    docFileNames = result.Value!;
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting document results from store.");
            }
            finally
            {
                loadDocumentIsBusy = false;
            }
        }

        private Task DisplayDocument(string docFileName)
        {

            return Task.CompletedTask;
        }

        //private EventCallback<List<DocumentEntity>> OnLoadDocumentsAsync()
        //{
        //    var documents = new List<DocumentEntity>() {
        //        new DocumentEntity { Id = "1", Name = "Empty PDF", FilePath = "/Pdf/empty-pdf.pdf" },
        //        new DocumentEntity { Id = "2", Name = "Ocular Center Invoice", FilePath = "/Pdf/fake-occular-center.pdf" },
        //        new DocumentEntity { Id = "3", Name = "Lamp Receipt", FilePath = "/Pdf/fake-lamp-receit.pdf" },
        //        new DocumentEntity { Id = "4", Name = "Kayak Certificate", FilePath = "/Pdf/fake-kayak-certificate.pdf" }
        //    };

        //    //SelectedDocument = documents.First();

        //    //UiState.DocumentSelection.AvailableDocuments = documents;
        //    //UiState.DocumentSelection.SelectedDocument = SelectedDocument;

        //    return documents;
        //}
    }

}
