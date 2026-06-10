using DemoAzAI.Application;
using DemoAzAI.Blazor.Services.Api;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging.Configuration;

namespace DemoAzAI.Blazor.Components.Pages
{
    public abstract class BasePage : ComponentBase
    {
        public string executionSteps { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
        }

        protected async Task RefreshUI()
        {
            await InvokeAsync(StateHasChanged);
            await Task.Yield();
        }

        public bool ApiClientResulIsOk<T>(ILogger logger, ApiClientResult<T> result)
        {
            if (result == null || result.Value is null)
            {
                logger.LogWarning($"Error: api call for {typeof(T).Name} returned no value");
            }

            if (result!.IsSuccess == false)
            {
                logger.LogError(result.ErrorMessage ?? "Unknown API error.");
    
                return false;
            }

            return true;
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

    }
}
