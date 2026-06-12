using DemoAzAI.Application;
using DemoAzAI.Blazor.Services.Api;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging.Configuration;
using System.Reflection;

namespace DemoAzAI.Blazor.Components.Pages
{
    public abstract class BasePage : ComponentBase
    {
        [Inject]
        private ApiClient apiClient { get; set; } = default!;

        public string executionSteps { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
        }

        protected async Task RefreshUI()
        {
            await InvokeAsync(StateHasChanged);
            await Task.Yield();
        }

        public async Task<ApiClientResult<TRequest>?> ApiClientGetAsync<TRequest>(string apiRoute, ILogger? logger = null)
        {
            var result = await apiClient.GetAsync<TRequest>(apiRoute);

            if (ApiClientResultValid(apiRoute, result))
            {
                return result;
            }

            return null;

        }

        public async Task<ApiClientResult<TResponse>?> ApiClientPostAsync<TRequest, TResponse>(string apiRoute, TRequest request, ILogger? logger = null)
        {
            var result = await apiClient.PostAsync<TRequest, TResponse>(apiRoute, request);

            if (ApiClientResultValid(apiRoute, result))
            {
                return result;
            }

            return null;
        }

        private bool ApiClientResultValid<TResponse>(string apiRoute, ApiClientResult<TResponse> result, ILogger? logger = null)
        {
            if (result is null)
            {
                if (logger != null)
                    logger.LogError($"Api Get {apiRoute} returned no result");

                return false;
            }

            if (result.Value is null)
            {
                if (logger != null)
                    logger.LogError($"Api Get {apiRoute} returned no value");

                return false;
            }

            if (result.IsSuccess == false)
            {
                if (logger != null)
                    logger.LogError($"Api Get {apiRoute} returned error message {result.ErrorMessage}");

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
