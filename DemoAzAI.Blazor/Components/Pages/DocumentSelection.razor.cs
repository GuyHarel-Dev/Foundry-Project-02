using DemoAzAI.Blazor.Services.StateContainers;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public partial class DocumentSelection
    {

        private async Task OnDocumentSelected(ChangeEventArgs e)
        {
            if (e.Value == null)
            {
                return;
            }

            var docId = e.Value.ToString();
            SelectedDocument = UiState.DocumentSelection.AvailableDocuments.First((doc) => doc.Id == docId);
            UiState.DocumentSelection.SelectedDocument = SelectedDocument;
        }

        private void UploadDocument()
        {

        }

        protected override async Task OnInitializedAsync()
        {
            var documents = new List<UiStateDocumentInfo>() {
            new UiStateDocumentInfo { Id = "1", FileName = "Empty PDF", FilePath = "/Pdf/empty-pdf.pdf" },
            new UiStateDocumentInfo { Id = "2", FileName = "Ocular Center Invoice", FilePath = "/Pdf/fake-occular-center.pdf" },
            new UiStateDocumentInfo { Id = "3", FileName = "Lamp Receipt", FilePath = "/Pdf/fake-lamp-receit.pdf" },
            new UiStateDocumentInfo { Id = "4", FileName = "Kayak Certificate", FilePath = "/Pdf/fake-kayak-certificate.pdf" }
        };

            UiState.DocumentSelection.AvailableDocuments = documents;
            UiState.DocumentSelection.SelectedDocument = documents.First();
            SelectedDocument = UiState.DocumentSelection.SelectedDocument;
        }
    }
}
