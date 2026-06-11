using DemoAzAI.Domain.Entities;
using DemoAzAI.Infrastructure.ExternalServices.Stores;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/filestore")]
    [ApiController]
    public class FileStoreController(
            IFileStoreService fileStoreService
        ) : ControllerBase
    {

        [HttpGet("documents")]
        public async Task<ActionResult<IEnumerable<string>>> GetAvailableDocuments()
        {
            var documents = await fileStoreService.GetAllDocumentNames();

            return Ok(documents);
        }

    }
}
