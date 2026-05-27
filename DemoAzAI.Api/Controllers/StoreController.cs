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
           return Ok(embeddingStoreService.Embeddings.FirstOrDefault(e => e.Guid == Guid.Parse(guid)));
        }
    }
}
