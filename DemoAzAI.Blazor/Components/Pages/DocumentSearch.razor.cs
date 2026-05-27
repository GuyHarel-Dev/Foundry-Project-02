using DemoAzAI.Common;
using DemoAzAI.Contracts.Requests;
using DemoAzAI.Contracts.Responses;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages;

public partial class DocumentSearch
{
    [Inject]
    private IWebHostEnvironment Env { get; set; } = default!;

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

            //StateContainer.Statistics = string.Empty;
            //StateContainer.ExecutionLog = string.Empty;

            StepTracer.ClearSteps();
            StepTracer.AddStep("BlazorUi", $"{nameof(AnalyzeDocument)}: Starting analysis for {StateContainer.PdfUrl} with model: {SelectedModel}");

            //StateContainer.ExecutionLog = StepTracer.GetSteps();

            await RefreshUI();

            var request = new AnalyzeDocumentRequestDto()
            {
                ModelName = SelectedModel,
                FileName = Path.GetFileName(StateContainer.PdfUrl),
                Bytes = await File.ReadAllBytesAsync(Path.Combine(Env.WebRootPath, StateContainer.PdfUrl.TrimStart('/')))

            };

            var result = await apiClient.PostAsync<AnalyzeDocumentRequestDto,
                AnalyzeDocumentResponseDto>("api/document/analyze", request);

            if (!result.IsSuccess)
            {
                //StateContainer.ExecutionLog = result.ErrorMessage ?? "Unknown API error.";
                return;
            }

            StepTracer.AddStep("BlazorUi", $"{nameof(AnalyzeDocument)}: Received analysis result for {StateContainer.PdfUrl} with model: {SelectedModel}");

            var responseDto = result.Value;

            if (responseDto == null)
            {
                //StateContainer.ExecutionLog = "API returned no data.";
                return;
            }

            ExtractDocumentsContent(responseDto);
            //StateContainer.ExecutionLog = StepTracer.GetSteps();
            //StateContainer.Statistics = ExtractStatistics(result.Value);

        }
        catch (Exception ex)
        {

            StateContainer.ExecutionLog = ex.Message;
            throw;
        }
        finally
        {
            isBusy = false;
        }

    }

    private void ExtractDocumentsContent(AnalyzeDocumentResponseDto? response)
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

    private string ExtractStatistics(AnalyzeDocumentResponseDto? response)
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