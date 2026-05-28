using DemoAzAI.Common;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public abstract class BasePage : ComponentBase
    {
        protected override async Task OnInitializedAsync()
        {
        }

        protected string AddUiStepperStep(string source, string title, bool clearSteps = false)
        {
            if(clearSteps) StepTracer.ClearSteps();

            StepTracer.AddStep(source, title);

            return StepTracer.GetSteps();

        }
        protected async Task RefreshUI()
        {
            await InvokeAsync(StateHasChanged);
            await Task.Yield();
        }
    }
}
