namespace DemoAzAI.Infrastructure.ExternalServices.Stores
{
    public interface IFileStoreService
    {
        Task<IEnumerable<string>> GetAllDocuments();
    }
}
