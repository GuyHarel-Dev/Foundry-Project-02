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
    //[Inject]
    //private IWebHostEnvironment Env { get; set; } = default!;

    [Inject]
    private ILogger<DocumentAnalysis> logger { get; set; } = default!;

    [Inject]
    private ApiClient apiClient { get; set; } = default!;

    [Inject]
    private IExecutionStepTracer executionStepTracer { get; set; } = default!;

    private async Task Test()
    {

    }

    private async Task OnDocumentSelected(ChangeEventArgs e)
    {
        documentSelectedName = e.Value?.ToString() ?? string.Empty;
    }

    private async Task OnAnalyzeDocument()
    {

        var request = new DocumentAnalysisRequestDto()
        {
            ModelName = SelectedModel,
            FileName = Path.GetFileName(documentSelectedName)
        };

        try
        {
            sendDocumentAnalysisBusy = true;

            var result = await apiClient.PostAsync<DocumentAnalysisRequestDto, DocumentAnalysisResponseDto>("api/document/analyze", request, logger);

            if (result is not null)
            {

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

    private void BeginAnalyseDocumentUiProcess()
    {
        sendDocumentAnalysisBusy = true;
        //AddExecutionStep(executionStepTracer, $"Starting analysis for document {SelectedDocument.FileName} using model {SelectedModel}");
    }

    private void EndAnalyseDocumentUiProcess()
    {
        sendDocumentAnalysisBusy = false;
        //AddExecutionStep(executionStepTracer, $"Analysis completed for document {SelectedDocument.FileName} using model {SelectedModel}");
    }


    private async Task SendAnalyzeDocumentRequest()
    {



    }

    private async Task SaveAnalysisToStore()
    {

        //if (UiState.DocumentAnalysis.DocumentAnalysis is null)
        //{
        //    AddExecutionError(executionStepTracer, "No document analysis available to save to store");
        //    return;
        //}

        //try
        //{
        //    var analysis = UiState.DocumentAnalysis.DocumentAnalysis;

        //    BeginStoreDocumentAnalysisUiProcess(analysis);

        //    var request = new StoredDocumentAnalysisRequestDto()
        //    {
        //        FileName = analysis.FileName,
        //        BytesHash = analysis.BytesHash,
        //        DocumentAnalysisResult = analysis.DocumentAnalysisResult
        //    };

        //    var result = await apiClient.PostAsync<StoredDocumentAnalysisRequestDto,
        //        StoredDocumentAnalysisResponseDto>("api/store/document/analysis", request);

        //    if (ApiClientResulIsOk(logger, result))
        //    {


        //    }

        //}
        //catch (Exception ex)
        //{
        //    Logger.LogError(ex, "Error while saving document analysis to store.");
        //}
        //finally
        //{
        //    EndStoreDocumentAnalysisUiProcess();
        //}

    }

    private void BeginStoreDocumentAnalysisUiProcess(DocumentAnalysisResponseDto analysis)
    {
        storeDocumentAnalysisBusy = true;
        AddExecutionStep(executionStepTracer, $"Saving document analysis for {analysis.FileName} to store.");
    }

    private void EndStoreDocumentAnalysisUiProcess()
    {
        storeDocumentAnalysisBusy = false;
        AddExecutionStep(executionStepTracer, $"Document saved to store.");
    }

    private void DisplayDocument(DocumentAnalysisResult? result)
    {
        if (result == null) return;

        documentExtractedText = result.DocContent;  //StateContainer.DocContent = result.DocContent;

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

    private string ExtractStatistics(DocumentAnalysisResponseDto? response)
    {
        if (response is null || response.DocumentAnalysisResult is null)
            return "No response data to extract statistics.";

        string str = "";

        str += $"PageCount: {response.DocumentAnalysisResult.PageCount}\n";
        str += $"ParagraphCount: {response.DocumentAnalysisResult.ParagraphCount}\n";
        str += $"TableCount: {response.DocumentAnalysisResult.TableCount}\n";
        str += $"FigureCount: {response.DocumentAnalysisResult.FigureCount}\n";
        str += $"SectionCount: {response.DocumentAnalysisResult.SectionCount}\n";
        str += $"LanguagesCount: {response.DocumentAnalysisResult.LanguagesCount}\n";
        str += $"LanguagesCount: {response.DocumentAnalysisResult.LanguagesCount}\n";

        // Derived statistics
        str += $"CharacterCount : {response.DocumentAnalysisResult.DocContent.Length}\n";
        str += $"WordCount : {response.DocumentAnalysisResult.DocContent.Split(' ').Length}\n";

        return str;

    }


}