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


        protected override async Task OnInitializedAsync()
        {
            await LoadDocumentNamesAsync();
        }

        private void OnDocumentSelected(ChangeEventArgs e)
        {
            var docFileName = e.Value?.ToString();

            if (string.IsNullOrEmpty(docFileName)) return;

            if (PageState.DocFileNames is null) return;

            PageState.DocFileNameSelected = PageState.DocFileNames.FirstOrDefault(name => name == docFileName);

        }

        private string DocFileNameUrl()
        {
            var url = string.Empty;

            if (PageState.DocFileNameSelected != null)
            {
                url = $"{apiClient.BaseClientUrl}/api/documents/content/{PageState.DocFileNameSelected}";
            }

            return url;
        }

        private async Task LoadDocumentNamesAsync()
        {
            try
            {

                var result = await apiClient.GetAsync<IEnumerable<string>>("api/documents/names", logger);

                if (result is not null)
                {
                    PageState.DocFileNames = result.Value!.ToList();
                    PageState.DocFileNameSelected = PageState.DocFileNames.First();
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting document results from store.");
            }
   
        }

    }

}
