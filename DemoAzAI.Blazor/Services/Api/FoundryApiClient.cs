namespace DemoAzAI.Blazor.Services.Api
{
    public sealed class FoundryApiClient
    {
        private readonly HttpClient _http;

        public FoundryApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<FoundryApiClientResult<TResponse>> PostAsync<TRequest, TResponse>(
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

                    return FoundryApiClientResult<TResponse>.Failure(message, (int)response.StatusCode);
                }

                var value = await response.Content.ReadFromJsonAsync<TResponse>(
                    cancellationToken: cancellationToken);

                return FoundryApiClientResult<TResponse>.Success(value);
            }
            catch (HttpRequestException ex)
            {
                return FoundryApiClientResult<TResponse>.Failure(
                    $"Network error while calling the API: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                return FoundryApiClientResult<TResponse>.Failure(
                    $"The API request timed out: {ex.Message}");
            }
            catch (Exception ex)
            {
                return FoundryApiClientResult<TResponse>.Failure(
                    $"Unexpected error: {ex.Message}");
            }
        }
    }

}
