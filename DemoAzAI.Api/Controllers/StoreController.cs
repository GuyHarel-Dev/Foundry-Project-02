using DemoAzAI.Abstractions;
using DemoAzAI.Domain;
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

        [HttpPost("document/analysis")]
        public IActionResult StoreDocument([FromBody] DocumentAnalysisResult document)
        {
            //storeService.SaveAsync(document);

            return Ok();
        }

    }
}
