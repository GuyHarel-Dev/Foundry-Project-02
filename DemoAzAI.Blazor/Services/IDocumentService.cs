using DemoAzAI.Domain.Models;

namespace DemoAzAI.Blazor.Services
{
    public interface IDocumentService
    {
        Task<IEnumerable<DocumentInfo>?> GetAllDocumentInfosFromStoreAsync(ILogger? logger = null, List<string>? errorMessages = null);
    }
}
