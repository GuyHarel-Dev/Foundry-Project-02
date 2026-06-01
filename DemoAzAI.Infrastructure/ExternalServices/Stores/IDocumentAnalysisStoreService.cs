using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAzAI.Infrastructure.ExternalServices.Stores
{
    public interface IDocumentAnalysisStoreService<TEntity>
    {
        Task SaveAsync(TEntity result);

        Task<TEntity?> GetAsync(string documentHash);

        Task<bool> ExistsAsync(string documentHash);
    }
}
