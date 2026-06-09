using DemoAzAI.Application;
using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Blazor.Services.StateContainers;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Models.DocumentAnalysis;
using Microsoft.AspNetCore.Components;
using System.Security.Cryptography;
using System.Text;

namespace DemoAzAI.Blazor.Components.Pages;

public partial class DocumentAnalysis
{
    [Inject]
    private IWebHostEnvironment Env { get; set; } = default!;

    [Inject]
    private ILogger<DocumentAnalysis> logger {  get; set; } = default!;

    [Inject]
    private ApiClient apiClient { get; set; } = default!;

    [Inject]
    private IExecutionStepTracer executionStepTracer { get; set; } = default!;

    private async Task OnDocumentSelected(ChangeEventArgs e)
    {
        if (e.Value == null)
        {
            return;
        }

        var docId = e.Value.ToString();
        SelectedDocument = UiState.DocumentSelection.AvailableDocuments.First((doc) => doc.Id == docId);

    }

    protected override async Task OnInitializedAsync()
    {
        if (UiState.DocumentSelection.AvailableDocuments.Any()) 
        {
            AvailableDocuments = UiState.DocumentSelection.AvailableDocuments;
            if (UiState.DocumentSelection.SelectedDocument is not null)
                SelectedDocument = UiState.DocumentSelection.SelectedDocument;
        }
    }

    private bool IsDocumentSelected(UiStateDocumentInfo docInfo)
    {
        return SelectedDocument.Id == docInfo.Id;
    }

    private async Task AnalyzeDocument()
    {
        try
        {

            BeginAnalyseDocumentUiProcess();

            var result = await SendAnalyzeDocumentRequest();

            if (result == null)
            {
                AddExecutionStep(executionStepTracer, $"No analysis result returned for {SelectedDocument.FileName} with model: {SelectedModel}", true); return;
            }

            DisplayDocument(result.DocumentAnalysisResult);

            EndAnalyseDocumentUiProcess();

        }
        catch (Exception ex)
        {
            LogExecutionError(logger, ex, "Error while processing document for analysis.");
        }
        finally
        {
            sendDocumentAnalysisBusy = false;
        }

    }

    private void BeginAnalyseDocumentUiProcess()
    {
        sendDocumentAnalysisBusy = true;
        AddExecutionStep(executionStepTracer, $"Starting analysis for document {SelectedDocument.FileName} using model {SelectedModel}");
    }

    private void EndAnalyseDocumentUiProcess()
    {
        sendDocumentAnalysisBusy = false;
        AddExecutionStep(executionStepTracer, $"Analysis completed for document {SelectedDocument.FileName} using model {SelectedModel}");
    }


    private async Task<DocumentAnalysisResponseDto?> SendAnalyzeDocumentRequest()
    {

        var request = new DocumentAnalysisRequestDto()
        {
            ModelName = SelectedModel,
            FileName = Path.GetFileName(SelectedDocument.FileName),
            Bytes = await File.ReadAllBytesAsync(Path.Combine(Env.WebRootPath, SelectedDocument.FilePath.TrimStart('/')))
        };

        request.BytesHash = Convert.ToHexString(SHA256.HashData(request.Bytes));

        var result = await apiClient.PostAsync<DocumentAnalysisRequestDto,
            DocumentAnalysisResponseDto>("api/document/analyse", request);

        UiState.DocumentAnalysis.DocumentAnalysis = result.Value;

        if (!result.IsSuccess)
        {
            AddExecutionStep(executionStepTracer, $"Error during analysis of {SelectedDocument.FilePath}. Message = {result.ErrorMessage}");
        }


        return result.Value;
    }

    private async Task SaveAnalysisToStore()
    {

        try
        {
            var analysis = UiState.DocumentAnalysis.DocumentAnalysis;

            if (analysis == null) 
                throw new ApplicationException("No document analysis available to save to store.");

            BeginStoreDocumentAnalysisUiProcess(analysis);

            var request = new StoredDocumentAnalysisRequestDto()
            {
                FileName = analysis.FileName,
                BytesHash = analysis.BytesHash,
                DocumentAnalysisResult = analysis.DocumentAnalysisResult
            };

            var result = await apiClient.PostAsync<StoredDocumentAnalysisRequestDto,
                StoredDocumentAnalysisResponseDto>("api/store/document/analysis", request);


            if (!result.IsSuccess)
            {
                AddExecutionStep(executionStepTracer, $"Error while saving {analysis.FileName} to store. Message = {result.ErrorMessage}");
            }

            EndStoreDocumentAnalysisUiProcess(analysis);


        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while saving document analysis to store.");
        }
        finally
        {
            storeDocumentAnalysisBusy = false;
        }

    }

    private void BeginStoreDocumentAnalysisUiProcess(DocumentAnalysisResponseDto analysis)
    {
        storeDocumentAnalysisBusy = true;
        AddExecutionStep(executionStepTracer, $"Saving document analysis for {analysis.FileName} to store.");
    }

    private void EndStoreDocumentAnalysisUiProcess(DocumentAnalysisResponseDto analysis)
    {
        storeDocumentAnalysisBusy = false;
        AddExecutionStep(executionStepTracer, $"Document {analysis.FileName} saved to store.");
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