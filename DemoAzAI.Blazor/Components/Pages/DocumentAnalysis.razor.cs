using DemoAzAI.Application;
using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Blazor.Services.UI;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages;

public partial class DocumentAnalysis
{
    [Inject]
    private IWebHostEnvironment Env { get; set; } = default!;

    [Inject]
    private FoundryApiClient apiClient { get; set; } = default!;

    [Inject]
    private IStepTracer StepTracer { get; set; } = default!;

    [Inject] 
    private DocumentSearchStateContainer StateContainer { get; set; } = default!;

    private bool isBusy;

    private string ReadModel = "prebuilt-read";
    private string LayoutModel = "prebuilt-layout";
    private string InvoiceModel = "prebuilt-invoice";
    private string ReceiptModel = "prebuilt-receipt";

    private string SelectedModel = "prebuilt-read";

    private string executionLog = string.Empty;
    private string statistics = string.Empty;


    private void OnModelRadioChange(ChangeEventArgs e)
    {
        SelectedModel = e.Value?.ToString() ?? "prebuilt-read";
        StateContainer.AiModel = SelectedModel;
    }

    protected string StatusMessage = "Ready";

    private async Task AnalyzeDocument()
    {
        try
        {
            isBusy = true;

            StateContainer.Statistics = string.Empty;
            StateContainer.ExecutionSteps = string.Empty;

            executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameof(AnalyzeDocument)}: Starting analysis for {StateContainer.PdfUrl} with model: {SelectedModel}", clearSteps: true);

            await RefreshUI();

            var request = new DocumenteAnalysisRequestDto()
            {
                ModelName = SelectedModel,
                FileName = Path.GetFileName(StateContainer.PdfUrl),
                Bytes = await File.ReadAllBytesAsync(Path.Combine(Env.WebRootPath, StateContainer.PdfUrl.TrimStart('/')))

            };

            var result = await apiClient.PostAsync<DocumenteAnalysisRequestDto,
                DocumentAnalysisResponseDto>("api/document/analyze", request);

            if (!result.IsSuccess)
            {
                StateContainer.ExecutionSteps = result.ErrorMessage ?? "Unknown API error.";
                return;
            }

            executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameof(AnalyzeDocument)}: Received analysis result for {StateContainer.PdfUrl} with model: {SelectedModel}");

            var responseDto = result.Value;

            if (responseDto == null)
            {
                StateContainer.ExecutionSteps = "API returned no data.";
                return;
            }

            ExtractDocumentsContent(responseDto);

            StateContainer.ExecutionSteps = StepTracer.GetSteps();
            StateContainer.Statistics = ExtractStatistics(result.Value);
            statistics = StateContainer.Statistics;

        }
        catch (Exception ex)
        {

            StateContainer.ExecutionSteps = ex.Message;
            throw;
        }
        finally
        {
            isBusy = false;
        }

    }

    private void ExtractDocumentsContent(DocumentAnalysisResponseDto? response)
    {
        if (response == null)
        {
            StateContainer.DocContent = "No response documents or fields to extract.";
            return;
        }

        StateContainer.DocContent = response.DocContent ?? "No document content extracted.";

        // Fields extraction

        if (response.Documents is null)
        {
            StateContainer.DocFields = "No documents fields found in the response.";
            return;
        }


        var contentBuilder = new System.Text.StringBuilder();

        foreach (var doc in response.Documents)
        {
            contentBuilder.AppendLine($"Document Type: {doc.DocumentType}");

            foreach (var field in doc.Fields)
            {
                contentBuilder.AppendLine();
                contentBuilder.AppendLine($"  Field Name: {field.Name}");
                contentBuilder.AppendLine($"  Field Value: {field.Value}");
                contentBuilder.AppendLine($"  Field Confidence: {field.Confidence}");
                contentBuilder.AppendLine($"  Field FieldType: {field.FieldType}");
            }
        }

        StateContainer.DocFields = contentBuilder.ToString();

    }

    private string ExtractStatistics(DocumentAnalysisResponseDto? response)
    {
        if (response == null)
            return "No response data to extract statistics.";

        string str = "";

        str += $"PageCount: {response.PageCount}\n";
        str += $"ParagraphCount: {response.ParagraphCount}\n";
        str += $"TableCount: {response.TableCount}\n";
        str += $"FigureCount: {response.FigureCount}\n";
        str += $"SectionCount: {response.SectionCount}\n";
        str += $"LanguagesCount: {response.LanguagesCount}\n";
        str += $"LanguagesCount: {response.LanguagesCount}\n";

        // Derived statistics
        str += $"CharacterCount : {response.DocContent.Length}\n";
        str += $"WordCount : {response.DocContent.Split(' ').Length}\n";

        return str;

    }
}