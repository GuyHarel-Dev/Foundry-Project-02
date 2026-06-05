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


    protected override async Task OnInitializedAsync()
    {
        DocumentSelected = UiState.DocumentSelected;
    }

    private async Task AnalyzeDocument()
    {
        try
        {
            sendDocumentAnalysBusy = true;
            await RefreshUI();

      
            var result = await SendAnalyzeDocumentRequest();

            if (result == null)
            {
                Logger.LogWarning($"No analysis result returned for {DocumentSelected} with model: {SelectedModel}");
                return;
            }

            DisplayDocument(result.DocumentAnalysisResult);

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

        var request = new DocumentAnalysisRequestDto()
        {
            ModelName = SelectedModel,
            FileName = Path.GetFileName(DocumentSelected),
            Bytes = await File.ReadAllBytesAsync(Path.Combine(Env.WebRootPath, DocumentSelected.TrimStart('/'))),
            AddDocumentToStore = true

        };

        var result = await apiClient.PostAsync<DocumentAnalysisRequestDto,
            DocumentAnalysisResponseDto>("api/document/analyse", request);

        if (!result.IsSuccess)
        {
            throw new ApplicationException($"{result.StatusCode.ToString()}, {result.ErrorMessage}");
        }


        return result.Value;
    }

   private async Task SaveAnalysisToStore()
    {

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