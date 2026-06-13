using DemoAzAI.Application;
using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Models.DocumentAnalysis;
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

    protected override async Task OnInitializedAsync()
    {
        await LoadDocumentNamesAsync();
    }

    private async Task OnDocumentSelected(ChangeEventArgs e)
    {
        documentSelectedName = e.Value?.ToString() ?? string.Empty;
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

    private async Task LoadDocumentNamesAsync()
    {
        try
        {
            var result = await apiClient.GetAsync<IEnumerable<string>>("api/documents/names", logger);

            if (result is not null)
            {
                docFileNames = result.Value!.ToList();
            }

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting document results from store.");
        }

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