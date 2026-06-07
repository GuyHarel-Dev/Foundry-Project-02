using DemoAzAI.Application;
using DemoAzAI.Blazor.Services.Api;
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
    private ApiClient apiClient { get; set; } = default!;

    [Inject]
    private IStepTracer StepTracer { get; set; } = default!;

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

    private async Task AnalyzeDocument()
    {
        try
        {
            StepTracer.ClearSteps();

            sendDocumentAnalysisBusy = true;

            AddStepBeginAnalysis();

            var result = await SendAnalyzeDocumentRequest();

            if (result == null)
            {
                Logger.LogWarning($"No analysis result returned for {SelectedDocument.FileName} with model: {SelectedModel}");
                return;
            }

            DisplayDocument(result.DocumentAnalysisResult);

            AddStepAnalysisCompleted();

        }
        catch (Exception ex)
        {
            StepTracer.AddUiStep(ex.Message);

            Logger.LogError(ex, "Error while processing document for analysis.");
        }
        finally
        {
            sendDocumentAnalysisBusy = false;
        }

        executionSteps = StepTracer.GetSteps();
    }

    private void AddStepBeginAnalysis()
    {
        StepTracer.AddUiStep($"Starting analysis for document {SelectedDocument.FileName} using model {SelectedModel}");
    }

    private void AddStepAnalysisCompleted()
    {
        StepTracer.AddUiStep($"Analysis completed for document {SelectedDocument.FileName} using model {SelectedModel}");
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
            StepTracer.AddStep(nameof(SaveAnalysisToStore), $"Error during analysis of {SelectedDocument.FilePath}. Message = {result.ErrorMessage}");
        }


        return result.Value;
    }

    private async Task SaveAnalysisToStore()
    {
        var stepSource = $"UI DocumentAnalysis:{nameof(SaveAnalysisToStore)}";

        try
        {
            storeDocumentAnalysisBusy = true;
            StepTracer.ClearSteps();

            var analysis = UiState.DocumentAnalysis.DocumentAnalysis;

            if (analysis == null) 
                throw new ApplicationException("No document analysis available to save to store.");

            StepTracer.AddStep(stepSource, $"Saving document analysis for {analysis.FileName} to store.");

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
                StepTracer.AddStep(stepSource, $"Error while saving {analysis.FileName} to store. Message = {result.ErrorMessage}");
            }

            StepTracer.AddStep(stepSource, $"Document {analysis.FileName} saved to store.");

        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while saving document analysis to store.");
        }
        finally
        {
            storeDocumentAnalysisBusy = false;
        }

        executionSteps = StepTracer.GetSteps();
    }


    private bool SaveAnalysisToStoreButtonDisabled()
    {
        return UiState.DocumentAnalysis.DocumentAnalysis == null;
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

        sb.AppendLine("Documents:");
        foreach (var doc in result.Documents)
        {
            sb.AppendLine($"  Document type: {doc.DocumentType}");
            sb.AppendLine($"  Document type confidence: {doc.DocumentTypeConfidence}");

            sb.AppendLine("    Document Fields:");
            foreach (var fields in doc.DocumentFields)
            {
                sb.AppendLine($"      Field name: {fields.Name}");
                sb.AppendLine($"      Field value: {fields.Value}");
                sb.AppendLine($"      Field type: {fields.FieldType}");
                sb.AppendLine($"      Field type confidence: {fields.FieldTypeConfidence}");
            }
        }

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