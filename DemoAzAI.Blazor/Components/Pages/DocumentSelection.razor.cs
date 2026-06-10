using DemoAzAI.Blazor.Models;

namespace DemoAzAI.Blazor.Components.Pages
{
    public partial class DocumentSelection
    {

        private void OnDocumentSelected(DocumentEntity doc)
        {
            //if (e.Value == null)
            //{
            //    return;
            //}

            //var docId = e.Value.ToString();
            //SelectedDocument = UiState.DocumentSelection.AvailableDocuments.First((doc) => doc.Id == docId);
            //UiState.DocumentSelection.SelectedDocument = SelectedDocument;

            var id = doc.Id; 
        }

        private void UploadDocument()
        {

        }

        private async Task<IEnumerable<DocumentEntity>> OnLoadDocumentsAsync()
        {
            var documents = new List<DocumentEntity>() {
                new DocumentEntity { Id = "1", Name = "Empty PDF", FilePath = "/Pdf/empty-pdf.pdf" },
                new DocumentEntity { Id = "2", Name = "Ocular Center Invoice", FilePath = "/Pdf/fake-occular-center.pdf" },
                new DocumentEntity { Id = "3", Name = "Lamp Receipt", FilePath = "/Pdf/fake-lamp-receit.pdf" },
                new DocumentEntity { Id = "4", Name = "Kayak Certificate", FilePath = "/Pdf/fake-kayak-certificate.pdf" }
            };

            return documents;
            //UiState.DocumentSelection.AvailableDocuments = documents;
            //UiState.DocumentSelection.SelectedDocument = documents.First();
            //SelectedDocument = UiState.DocumentSelection.SelectedDocument;
        }
    }
}
