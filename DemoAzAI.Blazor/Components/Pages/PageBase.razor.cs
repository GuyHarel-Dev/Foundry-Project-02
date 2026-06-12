using DemoAzAI.Application;
using DemoAzAI.Blazor.Services.Api;
using Microsoft.AspNetCore.Components;

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
