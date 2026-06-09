using DemoAzAI.Application;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public abstract class BasePage: ComponentBase
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

        public void AddExecutionStep(IExecutionStepTracer executionStepTracer, string title, bool logStep = false)
        {
            executionSteps = executionStepTracer.AddUiStep(title);
        }

        public void LogExecutionError(ILogger logger, Exception ex, string title)
        {
            logger.LogError(ex, title);

        }

    }
}
