using DemoAzAI.Application;
using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Blazor.Services.UI;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace DemoAzAI.Blazor.Components.Pages;

public partial class DocumentAnalysis
{
    [Inject]
    private IWebHostEnvironment Env { get; set; } = default!;

    [Inject]
    private FoundryApiClient apiClient { get; set; } = default!;

    [Inject]
    private IStepTracer StepTracer { get; set; } = default!;

    //[Inject] 
    //private DocumentSearchStateContainer StateContainer { get; set; } = default!;

    private bool isBusy;


    private void OnModelRadioChange(ChangeEventArgs e)
    {
        SelectedModel = e.Value?.ToString() ?? "prebuilt-read";
        //StateContainer.AiModel = SelectedModel;
    }

    protected string StatusMessage = "Ready";

    private async Task AnalyzeDocument()
    {
        try
        {
            isBusy = true;

            //StateContainer.Statistics = string.Empty;
            //StateContainer.ExecutionSteps = string.Empty;

            //executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameof(AnalyzeDocument)}: Starting analysis for {StateContainer.PdfUrl} with model: {SelectedModel}", clearSteps: true);
            executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameof(AnalyzeDocument)}: Starting analysis for {pdfUrl} with model: {SelectedModel}", clearSteps: true);

            await RefreshUI();

            var request = new DocumentAnalysisRequestDto()
            {
                ModelName = SelectedModel,
                FileName = Path.GetFileName(pdfUrl),
                Bytes = await File.ReadAllBytesAsync(Path.Combine(Env.WebRootPath, pdfUrl.TrimStart('/'))),
                AddDocumentToStore = true

            };

            var result = await apiClient.PostAsync<DocumentAnalysisRequestDto,
                DocumentAnalysisResponseDto>("api/document/analyse", request);

            if (!result.IsSuccess)
            {
                throw new ApplicationException($"{result.StatusCode.ToString()}, {result.ErrorMessage}");
            }

            executionLog = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameof(AnalyzeDocument)}: Received analysis result for {pdfUrl} with model: {SelectedModel}");

            var responseDto = result.Value;

            DisplayDocument(responseDto);

            executionLog = StepTracer.GetSteps();// StateContainer.ExecutionSteps = StepTracer.GetSteps();
            statistics = ExtractStatistics(responseDto);  //StateContainer.Statistics = ExtractStatistics(result.Value);
            //statistics = StateContainer.Statistics;

        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while processing document for analysis.");
        }
        finally
        {
            isBusy = false;
        }

    }

    private void DisplayDocument(DocumentAnalysisResponseDto? response)
    {
        if (response == null || response.DocumentAnalysisResult == null)
        {
            documentText = "Document analysis returned no data.";// StateContainer.DocContent = "Document analysis returned no data.";
            return;
        }

        var result = response.DocumentAnalysisResult;

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

        //documentContent = System.Text.Json.JsonSerializer.Serialize(result, new System.Text.Json.JsonSerializerOptions()
        //{
        //    WriteIndented = true
        //});
    }

    private string ExtractStatistics(DocumentAnalysisResponseDto? response)
    {
        if (response == null)
            return "No response data to extract statistics.";

        string str = "";

        //str += $"PageCount: {response.PageCount}\n";
        //str += $"ParagraphCount: {response.ParagraphCount}\n";
        //str += $"TableCount: {response.TableCount}\n";
        //str += $"FigureCount: {response.FigureCount}\n";
        //str += $"SectionCount: {response.SectionCount}\n";
        //str += $"LanguagesCount: {response.LanguagesCount}\n";
        //str += $"LanguagesCount: {response.LanguagesCount}\n";

        //// Derived statistics
        //str += $"CharacterCount : {response.DocContent.Length}\n";
        //str += $"WordCount : {response.DocContent.Split(' ').Length}\n";

        return str;

    }
}