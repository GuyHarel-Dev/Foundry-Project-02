using DemoAzAI.Blazor.Services.Api;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace DemoAzAI.Blazor.Components.Pages
{
    public abstract class BasePage : ComponentBase
    {
        [Inject]
        public ApiClient apiClient { get; set; } = default!;

        public List<string> BasePageSteps { get; set; } = new();
        public List<string> BasePageLog { get; set; } = new();

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
            if (BasePageLog.Any() == false)
                return string.Empty;

            var sb = new StringBuilder();
            foreach (string msg in BasePageLog)
            {
                sb.AppendLine(msg);
            }
            return sb.ToString();
        }

        public void AddBasePageExecutionError(string title, ILogger? logger = null)
        {
            BasePageLog.Add($"{title}");

            if (logger != null)
                logger.LogError(title);
        }

        public void AddBasePageExecutionStep(string title)
        {
            BasePageSteps.Add($"{title}");
        }


    }
}
