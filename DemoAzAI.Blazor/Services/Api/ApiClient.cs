namespace DemoAzAI.Blazor.Services.Api
{
    public sealed class ApiClient
    {
        public string BaseClientUrl { get; }

        private readonly HttpClient httpClient;

        public ApiClient(string url, HttpClient http)
        {
            BaseClientUrl = url;
            httpClient = http;
        }

        public async Task<ApiClientResult<TResponse>?> GetAsync<TResponse>
            (string apiRoute,
            ILogger? logger = null,
            List<string>? errorMessages = null)
        {
            var result = await HttpClientGetAsync<TResponse>(apiRoute);

            if (ApiClientResultValid(apiRoute, result, logger, errorMessages))
            {
                return result;
            }

            return null;

        }


        public async Task<ApiClientResult<TResponse>?> PostAsync<TRequest, TResponse>
            (string apiRoute,
            TRequest request,
            ILogger? logger = null,
            List<string>? errorMessages = null
            )
        {
            var result = await HttpClientPostAsync<TRequest, TResponse>(apiRoute, request);

            if (ApiClientResultValid(apiRoute, result, logger, errorMessages))
            {
                return result;
            }

            return null;
        }

        private bool ApiClientResultValid<TResponse>
            (string apiRoute,
            ApiClientResult<TResponse>
            result,
            ILogger? logger = null,
            List<string>? errorMessages = null)
        {
            if (result is null)
            {
                LogError($"Api Get {apiRoute} returned no result", logger, errorMessages);

                return false;
            }

            if (result.Value is null)
            {
                LogError($"Api {apiRoute} error: {result.ErrorMessage}, status:{result.StatusCode}", logger, errorMessages);

                return false;
            }

            if (result.IsSuccess == false)
            {
                LogError($"Api {apiRoute} returned error message {result.ErrorMessage}", logger, errorMessages);

                return false;
            }

            return true;
        }

        private void LogError(string message, ILogger? logger = null, List<string>? errorMessages = null)
        {
            if (logger != null)
                logger.LogError(message);

            if (errorMessages != null)
                errorMessages.Add(message);

        }

        public async Task<ApiClientResult<TResponse>> HttpClientGetAsync<TResponse>(
            string url,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await httpClient.GetAsync(url, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync(cancellationToken);
                    var message = response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.NotFound =>
                            "The requested API endpoint was not found.",
                        System.Net.HttpStatusCode.BadRequest =>
                            "The request was invalid.",
                        System.Net.HttpStatusCode.Unauthorized =>
                            "You are not authorized to use this feature.",
                        System.Net.HttpStatusCode.InternalServerError =>
                            "The server failed while processing the request.",
                        _ =>
                            $"API error {(int)response.StatusCode}: {response.ReasonPhrase}"
                    };
                    if (!string.IsNullOrWhiteSpace(body))
                    {
                        message += $" Details: {body}";
                    }
                    return ApiClientResult<TResponse>.Failure(message, (int)response.StatusCode);
                }
                var value = await response.Content.ReadFromJsonAsync<TResponse>(
                    cancellationToken: cancellationToken);

                return ApiClientResult<TResponse>.Success(value);
            }
            catch (HttpRequestException ex)
            {
                return ApiClientResult<TResponse>.Failure(
                    $"Network error while calling the API: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                return ApiClientResult<TResponse>.Failure(
                    $"The API request timed out: {ex.Message}");
            }
            catch (Exception ex)
            {
                return ApiClientResult<TResponse>.Failure(
                    $"Unexpected error: {ex.Message}");
            }
        }

        public async Task<ApiClientResult<TResponse>> HttpClientPostAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(url, request, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync(cancellationToken);

                    var message = response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.NotFound =>
                            "The requested API endpoint was not found.",

                        System.Net.HttpStatusCode.BadRequest =>
                            "The request was invalid.",

                        System.Net.HttpStatusCode.Unauthorized =>
                            "You are not authorized to use this feature.",

                        System.Net.HttpStatusCode.InternalServerError =>
                            "The server failed while processing the request.",

                        _ =>
                            $"API error {(int)response.StatusCode}: {response.ReasonPhrase}"
                    };

                    if (!string.IsNullOrWhiteSpace(body))
                    {
                        message += $" Details: {body}";
                    }

                    return ApiClientResult<TResponse>.Failure(message, (int)response.StatusCode);
                }

                var value = await response.Content.ReadFromJsonAsync<TResponse>(
                    cancellationToken: cancellationToken);

                return ApiClientResult<TResponse>.Success(value);
            }
            catch (HttpRequestException ex)
            {
                return ApiClientResult<TResponse>.Failure(
                    $"Network error while calling the API: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                return ApiClientResult<TResponse>.Failure(
                    $"The API request timed out: {ex.Message}");
            }
            catch (Exception ex)
            {
                return ApiClientResult<TResponse>.Failure(
                    $"Unexpected error: {ex.Message}");
            }
        }
    }

}
