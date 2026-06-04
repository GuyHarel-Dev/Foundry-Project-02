namespace DemoAzAI.Blazor.Services.Api
{
    public sealed class ApiClientResult<T>
    {
        public bool IsSuccess { get; init; }
        public T? Value { get; init; } = default(T?);
        public string? ErrorMessage { get; init; }
        public int? StatusCode { get; init; } 

        public static ApiClientResult<T> Success(T? value) =>
            new()
            {
                IsSuccess = true,
                Value = value
            };

        public static ApiClientResult<T> Failure(string errorMessage, int? statusCode = null) =>
            new()
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
    }

}
