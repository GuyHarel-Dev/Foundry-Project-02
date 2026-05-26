using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components
{
    public abstract class BasePage : ComponentBase
    {
        protected async Task RefreshUI()
        {
            await InvokeAsync(StateHasChanged);
            await Task.Yield();
        }
    }
}
