using DemoAzAI.Blazor.Services;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace DemoAzAI.Blazor.Components.Pages;

public partial class Analyze
{

    [Inject]
    private ILogger<Analyze> logger { get; set; } = default!;

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


    private async Task OnDocumentSelected(ChangeEventArgs e)
    {
        documentSelectedName = e.Value?.ToString() ?? string.Empty;

        var response = await documentService.GetDocumentAnalysisByNameAsync(documentSelectedName, logger, BasePageLog);

        if (response?.DocumentAnalysisEntity is null)
        {
            BasePageLog.Add($"Document selected: {documentSelectedName} was not found in the document result service. Start a document analysis to add to the repository");
            return;
        }

        DisplayResponse(response);
    }

    private async Task OnAnalyzeDocument()
    {

        var request = new DocumentAnalysisRequestDto()
        {
            ModelName = selectedModel,
            FileName = Path.GetFileName(documentSelectedName),
            SaveToStore = saveAnalysisToStore
        };

        try
        {
            sendDocumentAnalysisBusy = true;

            var response = await apiClient.PostAsync<DocumentAnalysisRequestDto, DocumentAnalysisResponseDto>
                ("api/document-analysis", 
                request, logger, BasePageLog);

            if (response is not null && response.Value is not null)
            {
                DisplayResponse(response.Value);
            }

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while analyzing the document.");
        }
        finally
        {
            sendDocumentAnalysisBusy = false;
        }

    }



    private void DisplayResponse(DocumentAnalysisResponseDto response)
    {
        documentExtractedText = response.DocumentAnalysisEntity?.DocumentAnalysisResult?.DocContent;

        var entity = response.DocumentAnalysisEntity;

        if (entity is null || entity.DocumentAnalysisResult is null)
        {
            documentAnalysisError = response.DocumentAnalysisStatus;
            return;
        }

        var result = entity.DocumentAnalysisResult;


        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"Document Format: {result.DocFormat}");
        sb.AppendLine($"Pages: {result.PageCount}");
        sb.AppendLine($"Paragraphs: {result.ParagraphCount}");
        sb.AppendLine($"Sections: {result.SectionCount}");
        sb.AppendLine($"Tables: {result.TableCount}");
        sb.AppendLine($"Figures: {result.FigureCount}");
        sb.AppendLine($"Languages: {result.LanguagesCount}");
        sb.AppendLine("");

        sb.AppendLine("Documents and their fields:");
        sb.AppendLine(result.DocumentsFieldsContent);

        documentExtractedSections = sb.ToString();


    }

}