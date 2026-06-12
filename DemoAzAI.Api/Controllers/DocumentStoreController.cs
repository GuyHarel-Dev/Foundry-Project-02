using DemoAzAI.Domain.Entities;
using DemoAzAI.Infrastructure.ExternalServices.Stores;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentStoreController(
            IDocumentStoreService fileStoreService
        ) : ControllerBase
    {

        [HttpGet("names")]
        public async Task<ActionResult<IEnumerable<string>>> GetDocumentNames()
        {
            var documents = await fileStoreService.GetDocumentNames();

            return Ok(documents);
        }

        [HttpGet("content/{fileName}")]
        public async Task<IActionResult> GetDocumentFile(string fileName)
        {
            var stream = await fileStoreService.GetDocumentFileStream(fileName);

            return File(stream, "application/pdf");

        }
    }
}
