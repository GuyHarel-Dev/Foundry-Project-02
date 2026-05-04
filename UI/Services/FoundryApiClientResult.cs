namespace UI.Services
{
    public sealed class FoundryApiClientResult<T>
    {
        public bool IsSuccess { get; init; }
        public T? Value { get; init; }
        public string? ErrorMessage { get; init; }
        public int? StatusCode { get; init; }

        public static FoundryApiClientResult<T> Success(T? value) =>
            new()
            {
                IsSuccess = true,
                Value = value
            };

        public static FoundryApiClientResult<T> Failure(string errorMessage, int? statusCode = null) =>
            new()
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
    }

}
