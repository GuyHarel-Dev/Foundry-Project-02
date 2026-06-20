using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public partial class Semantic
    {


        [Inject]
        private ILogger<Semantic> logger { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetDocumentEmbeddingsAsync();
                StateHasChanged();
            }

        }

        private async Task GetDocumentEmbeddingsAsync()
        {
            try
            {

                var result = await apiClient.GetAsync<IEnumerable<DocumentInfo>>("api/knowledge/embeddings", logger, BasePageLog);

                if (result is not null)
                {
                    storedEmbeddings = result.Value!;
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting document embeddings from store.");
            }
            finally
            {

            }
        }

        private async Task OnDocumentSelected(ChangeEventArgs e)
        {
            if (e.Value == null)
            {
                return;
            }

            var docId = e.Value.ToString();

            if (docId != null)
            {
                selectedDocumentEmbeddingsEntity = await GetDocumentEmbeddingByNameAsync(docId);
                documentKnowledge = selectedDocumentEmbeddingsEntity?.ToString();
            }

        }

        private async Task<DocumentKnowledgeEntity?> GetDocumentEmbeddingByNameAsync(string docId)
        {
            try
            {
                var result = await apiClient.GetAsync<DocumentKnowledgeEntity>($"api/knowledge/embeddings/{docId}", logger, BasePageLog);

                return result?.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting document analysis from store.");
            }
            finally
            {

            }

            return null;
        }


        private async Task SemanticSearch()
        {
            //if (string.IsNullOrWhiteSpace(semanticQuestion))
            //{
            //    semanticQuestion = "Please enter a semantic question...";
            //    return;
            //}

            //searchSteps = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameof(SemanticSearch)}: Searching embedding store...");

            //var request = new EmbeddingRequestDto
            //{
            //    AddEmbeddingsToStore = false,
            //    TextLines = semanticQuestion
            //        .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
            //        .ToList()
            //};

            // api/embedding/search/store


            await RefreshUIAsync();
        }

    }
}

