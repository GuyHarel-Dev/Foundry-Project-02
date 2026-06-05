using DemoAzAI.Application;
using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Models.DocumentAnalysis;
using Microsoft.AspNetCore.Components;
using System.Text;
using DemoAzAI.Blazor.Services.UI;

namespace DemoAzAI.Blazor.Components.Pages;

public partial class DocumentAnalysis
{
    [Inject]
    private IWebHostEnvironment Env { get; set; } = default!;

    [Inject]
    private ApiClient apiClient { get; set; } = default!;

    [Inject]
    private IStepTracer StepTracer { get; set; } = default!;

    [Inject]
    private UiStateContainer UiState { get; set;  } = default!;


    private void Test()
    {
        var abc = 123;

    }

    protected override async Task OnInitializedAsync()
    {
        DocumentSelected = UiState.DocumentSelected;
    }

    private async Task OnDocumentSelected(ChangeEventArgs e)
    {
        int id = Convert.ToInt16(e.Value);

        var result = await apiClient.GetAsync<DocumentAnalysisStoredResult>($"api/store/document/analysis/results/{id}");    

        if (result.Value == null)
        {
            executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"OnDocumentSelected: No stored results found for id {id}.");
            return;
        }
        SelectedDocument = _demoDocumentEmpty;
        DisplayDocument(result.Value.AnalysisResult);
    }

    private async Task AnalyzeDocument()
    {
        var nameOfMethod = nameof(AnalyzeDocument);

        try
        {
            sendDocumentAnalysBusy = true;

            executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameOfMethod}: Starting analysis for {SelectedDocument} with model: {SelectedModel}", clearSteps: true);

            await RefreshUI();

      
            var result = await SendAnalyzeDocumentRequest();

            if (result == null)
            {
                executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameOfMethod}: No analysis result returned for {SelectedDocument} with model: {SelectedModel}");
                return;
            }

            DisplayDocument(result.DocumentAnalysisResult);

            executionLog = StepTracer.GetSteps();
            statistics = ExtractStatistics(result);

        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while processing document for analysis.");
        }
        finally
        {
            sendDocumentAnalysBusy = false;
        }

    }

    private async Task<DocumentAnalysisResponseDto?> SendAnalyzeDocumentRequest()
    {
        var nameOfMethod = nameof(SendAnalyzeDocumentRequest);

        var request = new DocumentAnalysisRequestDto()
        {
            ModelName = SelectedModel,
            FileName = Path.GetFileName(SelectedDocument),
            Bytes = await File.ReadAllBytesAsync(Path.Combine(Env.WebRootPath, SelectedDocument.TrimStart('/'))),
            AddDocumentToStore = true

        };

        var result = await apiClient.PostAsync<DocumentAnalysisRequestDto,
            DocumentAnalysisResponseDto>("api/document/analyse", request);

        if (!result.IsSuccess)
        {
            throw new ApplicationException($"{result.StatusCode.ToString()}, {result.ErrorMessage}");
        }

        executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameOfMethod}: Received analysis result for {SelectedDocument} with model: {SelectedModel}");

        var responseDto = result.Value;

        if (responseDto == null)
        {
            executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameOfMethod}: Document analysis result status: no analyis value returned");
        }
        else
        {
            executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameOfMethod}: Document analysis result status: {responseDto.Status}");
        }

        return result.Value;
    }

    private async Task GetAllDocumentAnalysisResults()
    {
        var nameOfMethod = nameof(GetAllDocumentAnalysisResults);
        try
        {
            getStoredResultsBusy = true;

            executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameOfMethod}: Fetching results from store.", clearSteps: true);

            await RefreshUI();

            var request = new DocumentAnalysisRequestDto()
            {
                ModelName = SelectedModel,
                FileName = Path.GetFileName(SelectedDocument),
                Bytes = await File.ReadAllBytesAsync(Path.Combine(Env.WebRootPath, SelectedDocument.TrimStart('/'))),
                AddDocumentToStore = true

            };

            var result = await apiClient.GetAsync<List<DocumentAnalysisStoredResult>>("api/store/document/analysis/results");


            if (!result.IsSuccess)
            {
                throw new ApplicationException($"{result.StatusCode.ToString()}, {result.ErrorMessage}");
            }

            if ( result.Value == null || result.Value.Count == 0)
            {
                executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameOfMethod}: No stored results found.");
            }
            else
            {
                _storedResults = result.Value;
                executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameOfMethod}: Retrieved {result.Value.Count} stored results.");
            }
             
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while extraction document results from store.");
        }
        finally
        {
            getStoredResultsBusy = false;
        }


    }

    private async Task GetStoredResultByIndex(int index)
    {
        var nameOfMethod = nameof(GetStoredResultByIndex);
        try
        {
            getStoredResultsBusy = true;

            executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameOfMethod}: Fetching results from store.", clearSteps: true);

            await RefreshUI();

            var request = new DocumentAnalysisRequestDto()
            {
                ModelName = SelectedModel,
                FileName = Path.GetFileName(SelectedDocument),
                Bytes = await File.ReadAllBytesAsync(Path.Combine(Env.WebRootPath, SelectedDocument.TrimStart('/'))),
                AddDocumentToStore = true

            };

            var result = await apiClient.GetAsync<List<DocumentAnalysisStoredResult>>("api/store/document/analysis");


            if (!result.IsSuccess)
            {
                throw new ApplicationException($"{result.StatusCode.ToString()}, {result.ErrorMessage}");
            }

            if (result.Value == null || result.Value.Count == 0)
            {
                executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameOfMethod}: No stored results found.");
            }
            else
            {
                _storedResults = result.Value;
                executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameOfMethod}: Retrieved {result.Value.Count} stored results.");
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while extraction document results from store.");
        }
        finally
        {
            getStoredResultsBusy = false;
        }


    }

    private void DisplayDocument(DocumentAnalysisResult? result)
    {
        if (result == null) return;

        documentText = result.DocContent;  //StateContainer.DocContent = result.DocContent;

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

        documentContent = sb.ToString();

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