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

        //[HttpPost("document/analysis")]
        //public async Task<IActionResult> StoreDocument([FromBody] DocumentAnalysisResult document)
        //{
        //    await storeService.SaveAsync(document.FileName, document);

        //    return Ok();
        //}

        [HttpGet("document/analysis/history")]
        public async Task<ActionResult<DocumentAnalysisHistoryItem>> GetDocumentAnalysisHistory()
        {
            var history = await storeService.GetAllAsync();
            return Ok(history);
        }
    }
}
