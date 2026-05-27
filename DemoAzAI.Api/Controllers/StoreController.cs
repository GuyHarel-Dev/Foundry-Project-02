using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Store;

namespace WebApi.Controllers
{
    [Route("api/store")]
    [ApiController]
    public class StoreController (IEmbeddingStoreService embeddingStoreService) : ControllerBase
    {

        [HttpGet]
        [Route("embeddingvalues")]
        public ActionResult<StoredEmbedding> Get(string guid)
        {
            if (string.IsNullOrEmpty(guid) ||!embeddingStoreService.Embeddings.Exists( e => e.Guid.Equals(Guid.Parse(guid))))
            {
                return NotFound($"Guid not found in in-memory embedding store: {guid}");
            }

            return Ok(embeddingStoreService.Embeddings.First(e => e.Guid == Guid.Parse(guid)));
        }
    }
}
