namespace DemoAzAI.Infrastructure.ExternalServices.Repositories
{
    public sealed class DocumentAlreadyExistsException : Exception
    {
        public DocumentAlreadyExistsException(string fileName)
            : base($"Document '{fileName}' already exists.")
        {
        }
    }
}
