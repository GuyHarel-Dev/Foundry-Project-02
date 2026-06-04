namespace DemoAzAI.Blazor.Services.Api
{
    public sealed class ApiClient
    {
        private readonly HttpClient _http;

        public ApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<ApiClientResult<TResponse>> GetAsync<TResponse>(
            string url,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _http.GetAsync(url, cancellationToken);
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

        public async Task<ApiClientResult<TResponse>> PostAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _http.PostAsJsonAsync(url, request, cancellationToken);

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
