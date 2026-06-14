using DemoAzAI.Application;
using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace DemoAzAI.Blazor.Components.Pages;

public partial class DocumentAnalysis
{

    [Inject]
    private ILogger<DocumentAnalysis> logger { get; set; } = default!;

    [Inject]
    private ApiClient apiClient { get; set; } = default!;

    [Inject]
    private IExecutionStepTracer executionStepTracer { get; set; } = default!;

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

    private async Task OnDocumentSelected(ChangeEventArgs e)
    {
        documentSelectedName = e.Value?.ToString() ?? string.Empty;

        var docInfo = await GetDocumentAnalysisByNameAsync(documentSelectedName);

        documentExtractedText = string.Empty;
        documentExtractedSections = string.Empty;

        if (docInfo != null)
        {
            documentExtractedText = docInfo.AnalysisResult.DocContent;
            documentExtractedSections = docInfo.AnalysisResult.DocumentsFieldsContent;
        }
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
            BeginAnalyseDocumentUiProcess(request);

            var response = await apiClient.PostAsync<DocumentAnalysisRequestDto, DocumentAnalysisResponseDto>("api/document/analyze", request, logger);

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
            EndAnalyseDocumentUiProcess(request);
        }

    }

    private async Task<DocumentAnalysisEntity?> GetDocumentAnalysisByNameAsync(string fileName)
    {
        try
        {
            var result = await apiClient.GetAsync<DocumentAnalysisEntity>($"api/document-analysis/analysis/{fileName}", logger);

            if (result is not null)
            {
                var response = result.Value;

                return response;
            }
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
    private void BeginAnalyseDocumentUiProcess(DocumentAnalysisRequestDto request)
    {
        sendDocumentAnalysisBusy = true;
        AddExecutionStep(executionStepTracer, $"Starting analysis for document {request.FileName} using model {request.ModelName}");
    }

    private void EndAnalyseDocumentUiProcess(DocumentAnalysisRequestDto request)
    {
        sendDocumentAnalysisBusy = false;
        AddExecutionStep(executionStepTracer, $"Analysis completed for document {request.FileName} using model {request.ModelName}");
    }

    private void DisplayResponse(DocumentAnalysisResponseDto response)
    {
        if (response.DocumentAnalysisResult is null)
        {
            documentAnalysisError = response.DocumentAnalysisStatus;
            return;
        }

        var result = response.DocumentAnalysisResult;


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