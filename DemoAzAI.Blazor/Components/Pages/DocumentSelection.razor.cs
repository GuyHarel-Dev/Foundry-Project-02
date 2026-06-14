using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public partial class DocumentSelection
    {

        [Inject]
        private ILogger<DocumentSelection> logger { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                 var storeInfos = await GetAllDocumentInfosFromStoreAsync(logger);

                if (storeInfos != null)
                    storedDocumentInfo = [.. storeInfos];

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


    }

}
