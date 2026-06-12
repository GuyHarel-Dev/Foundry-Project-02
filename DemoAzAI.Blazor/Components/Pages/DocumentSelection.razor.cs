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


        private void OnDocumentSelected(ChangeEventArgs e)
        {
            var docFileName = e.Value?.ToString();

            if (string.IsNullOrEmpty(docFileName)) return;

            if (docFileNames is null) return;

            docFileNameSelected = docFileNames.FirstOrDefault(name => name == docFileName);

        }

        private string DocFileNameUrl()
        {
            var url = string.Empty;

            if (docFileNameSelected != null)
            {
                url = $"{apiClient.BaseClientUrl}/api/documents/content/{docFileNameSelected}";
            }

            return url;
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

    }

}
