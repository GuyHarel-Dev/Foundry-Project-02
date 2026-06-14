using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;

namespace DemoAzAI.Infrastructure.ExternalServices.Stores
{
    public interface IDocumentStoreService
    {
        List<DocumentInfo> GetDocumentFileInfos();

        Stream GetDocumentFileStream(string fileName);
    }
}
