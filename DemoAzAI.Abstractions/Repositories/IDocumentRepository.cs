using DemoAzAI.Abstractions.Models;

namespace DemoAzAI.Abstractions.Repositories
{
    public interface IDocumentRepository
    {
        List<DocumentEntity> GetAllDocuments();
    }
}
