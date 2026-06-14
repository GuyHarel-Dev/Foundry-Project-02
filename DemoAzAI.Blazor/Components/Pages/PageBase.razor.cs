using DemoAzAI.Application;
using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public abstract class BasePage : ComponentBase
    {
        [Inject]
        public ApiClient apiClient { get; set; } = default!;

        public string executionSteps { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
        }

        protected async Task RefreshUIAsync()
        {
            await InvokeAsync(StateHasChanged);
            await Task.Yield();
        }

        public void AddExecutionError(IExecutionStepTracer executionStepTracer, string title)
        {
            executionSteps = executionStepTracer.AddUiStep($"Error: {title}");

        }
        public void AddExecutionStep(IExecutionStepTracer executionStepTracer, string title)
        {
            executionSteps = executionStepTracer.AddUiStep(title);
        }

        public void LogExecutionError(ILogger logger, Exception ex, string title)
        {
            logger.LogError(ex, title);

        }

        public async Task<IEnumerable<DocumentInfo>?> GetAllDocumentInfosFromStoreAsync(ILogger logger)
        {
            try
            {
                var result = await apiClient.GetAsync<IEnumerable<DocumentInfo>>("api/document-store", logger);

                if (result is not null && result.Value is not null)
                {
                   return [.. result.Value];
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting document results from store.");
            }

            return null;
        }

    }
}
