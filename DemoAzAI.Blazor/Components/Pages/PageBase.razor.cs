using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Domain.Models;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace DemoAzAI.Blazor.Components.Pages
{
    public abstract class BasePage : ComponentBase
    {
        [Inject]
        public ApiClient apiClient { get; set; } = default!;

        public List<string> PageBaseExecutionSteps { get; set; } = new();
        public List<string> PageBaseExecutionErrors { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
        }

        protected async Task RefreshUIAsync()
        {
            await InvokeAsync(StateHasChanged);
            await Task.Yield();
        }

        public string GetPageBaseExecutionErrorsString()
        {
            var sb = new StringBuilder();
            foreach (string msg in PageBaseExecutionErrors)
            {
                sb.AppendLine(msg);
            }
            return sb.ToString();
        }

        public void AddExecutionError(string title)
        {
            PageBaseExecutionErrors.Add($"{title}");

        }
        public void AddExecutionStep(string title)
        {
            PageBaseExecutionSteps.Add($"{title}");
        }


    }
}
