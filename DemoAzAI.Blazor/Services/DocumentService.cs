using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Domain.Models;

namespace DemoAzAI.Blazor.Services
{
    public class DocumentService 
        (
            ApiClient apiClient
        ) : IDocumentService
    {
        public async Task<IEnumerable<DocumentInfo>?> GetAllDocumentInfosFromStoreAsync(ILogger? logger, List<string>? errorMessages)
        {
            try
            {
                var result = await apiClient.GetAsync<IEnumerable<DocumentInfo>>("api/document-store", logger, errorMessages);

                if (result is not null && result.Value is not null)
                {
                    return [.. result.Value];
                }

            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error while getting document results from store.");
            }

            return null;
        }
    }
}
