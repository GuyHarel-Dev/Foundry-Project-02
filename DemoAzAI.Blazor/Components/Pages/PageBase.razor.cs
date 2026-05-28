using DemoAzAI.Application;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public abstract class BasePage : ComponentBase
    {
       
        protected override async Task OnInitializedAsync()
        {
        }

        protected string AddUiStepperStep(IStepTracer stepTracer, string source, string title, bool clearSteps = false)
        {
            if(clearSteps) stepTracer.ClearSteps();

            stepTracer.AddStep(source, title);

            return stepTracer.GetSteps();

        }
        protected async Task RefreshUI()
        {
            await InvokeAsync(StateHasChanged);
            await Task.Yield();
        }
    }
}
