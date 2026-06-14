using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public partial class DocumentSelection
    {
        [Inject]
        public ApiClient apiClient { get; set; } = default!;

        [Inject]
        private ILogger<DocumentSelection> logger { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetDocumentInfosAsync();
                StateHasChanged();
            }
        }

        private void OnDocumentSelected(ChangeEventArgs e)
        {
            var docFileName = e.Value?.ToString();

            selectedDocumentInfo = storedDocumentInfo.Where(doc => doc.FileName == docFileName).FirstOrDefault();


        }

        private string GetDocumentInfoApiRoute()
        {
            var url = string.Empty;

            if (selectedDocumentInfo != null)
            {
                url = $"{apiClient.BaseClientUrl}/api/document-store/stream/{selectedDocumentInfo.FileName}";
            }

            return url;

        }
        private async Task GetDocumentInfosAsync()
        {
            try
            {
                var result = await apiClient.GetAsync<IEnumerable<DocumentInfo>>("api/document-store", logger);

                if (result is not null && result.Value is not null)
                {
                    storedDocumentInfo = [.. result.Value];
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting document results from store.");
            }
   
        }

    }

}
