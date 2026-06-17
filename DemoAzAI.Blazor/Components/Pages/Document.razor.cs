using DemoAzAI.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public partial class Document
    {

        [Inject]
        private ILogger<Document> logger { get; set; } = default!;

        [Inject]
        private IDocumentService documentService { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                 var storeInfos = await documentService.GetAllDocumentInfosFromStoreAsync(logger, BasePageLog);

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
                url = $"{apiClient.BaseClientUrl}/api/store/document/stream/{selectedDocumentInfo.FileName}";
            }

            return url;

        }


    }

}
