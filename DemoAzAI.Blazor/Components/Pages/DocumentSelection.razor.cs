
using DemoAzAI.Abstractions.Models;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public partial class DocumentSelection
    {

        private async Task OnDocumentSelected()
        {
            //SelectedDocument = doc;
            //UiState.DocumentSelection.SelectedDocument = SelectedDocument;
        }

        private async Task UploadDocument()
        {

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
