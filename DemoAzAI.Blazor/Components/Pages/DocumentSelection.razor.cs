
using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public partial class DocumentSelection
    {
        [Inject]
        private ILogger<DocumentSelection> logger { get; set; } = default!;

        [Inject]
        private ApiClient apiClient { get; set; } = default!;
        private async Task OnDocumentSelected(ChangeEventArgs e)
        {
            //SelectedDocument = doc;
            //UiState.DocumentSelection.SelectedDocument = SelectedDocument;
        }

        private async Task LoadDocumentsAsync()
        {
            try
            {

                loadDocumentIsBusy = true;


                var result = await apiClient.GetAsync<IEnumerable<StoreDocument>>("api/filestore/documents");

                if (ApiClientResulIsOk(logger, result))
                {
                    documents = result.Value!;
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
