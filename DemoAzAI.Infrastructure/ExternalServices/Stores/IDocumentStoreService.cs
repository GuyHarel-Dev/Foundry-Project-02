using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Infrastructure.ExternalServices.Stores
{
    public interface IDocumentStoreService
    {
        List<string> GetDocumentFileNames();

        Stream GetDocumentFileStream(string fileName);
    }
}
