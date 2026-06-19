using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public partial class Knowledge
    {

        [Inject]
        ILogger<Knowledge> logger { get; set; } = default!;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetDocumentResultsAsync();
                StateHasChanged();
            }

        }

        private async Task GetDocumentResultsAsync()
        {
            try
            {

                var result = await apiClient.GetAsync<IEnumerable<DocumentInfo>>("api/document/analysis", logger, BasePageLog);

                if (result is not null)
                {
                    storedDocuments = result.Value!.ToList();
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting document results from store.");
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
                selectedDocumentAnalysisEntity = await GetDocumentAnalysisByIdAsync(docId);
            }

        }

        private async Task<DocumentAnalysisEntity?> GetDocumentAnalysisByIdAsync(string docId)
        {
            try
            {
                var result = await apiClient.GetAsync<DocumentAnalysisResponseDto>($"api/document/analysis/id/{docId}", logger, BasePageLog);
                
                return result?.Value?.DocumentAnalysisEntity;
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

        private string? GetSelectedDocumentEntityAnalysisResult()
        {
            return selectedDocumentAnalysisEntity?.DocumentAnalysisResult?.DocContent;
        }
        private async Task ExtractKnowledgeFromText()
        {
            try
            {

                if (selectedDocumentAnalysisEntity is null)
                {
                    AddBasePageExecutionError($"Select a document stored result first", logger);
                    return;
                }

                sendKnowledgeExtractionRequestBusy = true;

                var request = new DocumentKnowledgeRequestDto()
                {
                    FileName = selectedDocumentAnalysisEntity.FileName,
                    TextToChunk = selectedDocumentAnalysisEntity?.DocumentAnalysisResult?.DocContent ?? string.Empty,
                    SaveToStore = saveKnowledgeChunksToStore
                };

                var result = await apiClient.PostAsync<DocumentKnowledgeRequestDto, DocumentKnowledgeResponseDto>
                    ("api/knowledge/embed", request, logger, BasePageLog);

                //documentKnowledgeChunks = result?.Value?.TextChunks;
                documentKnowledgeChunksStatistics = result?.Value?.ChunkingStatistics?.ToString();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting document results from store.");
            }
            finally
            {
                sendKnowledgeExtractionRequestBusy = false;
            }

        }


    }


}

