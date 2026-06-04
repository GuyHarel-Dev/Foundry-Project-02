using DemoAzAI.Abstractions;
using DemoAzAI.Domain.Models.DocumentAnalysis;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/store")]
    [ApiController]
    public class StoreController(
        IDocumentAnalysisStoreService storeService
        ) : ControllerBase
    {

        [HttpGet("document/analysis")]
        public async Task<ActionResult<IEnumerable<DocumentAnalysisStoredResult>>> GetAllDocumentAnalysis()
        {
            var history = await storeService.GetAllAsync();

            if (history is null || history.Any() is false)
            {
                return NotFound();
            }

            return Ok(history);
        }


        [HttpGet("document/analysis/{id}")]
        public async Task<ActionResult<DocumentAnalysisStoredResult>> GetDocumentAnalysis(string id)
        {
            var doc = await storeService.GetByIdAsync(id);

            if (doc is null)
            {
                return NotFound();
            }
            return Ok(doc);
        }


    }
}
