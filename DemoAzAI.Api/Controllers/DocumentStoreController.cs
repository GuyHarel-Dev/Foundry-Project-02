using DemoAzAI.Domain.Models;
using DemoAzAI.Infrastructure.ExternalServices.Stores;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/document-store")]
    [ApiController]
    public class DocumentStoreController(
            IDocumentStoreService fileStoreService
        ) : ControllerBase
    {

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<DocumentInfo>>> GetDocumentNames()
        {
            var documents = fileStoreService.GetDocumentFileInfos();

            return Ok(documents);
        }

        [HttpGet("stream/{fileName}")]
        public async Task<IActionResult> GetDocumentFile(string fileName)
        {
            var stream = fileStoreService.GetDocumentFileStream(fileName);

            return File(stream, "application/pdf");

        }
    }
}
