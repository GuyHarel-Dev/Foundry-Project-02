//using DemoAzAI.Domain;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using WebApi.Services.Interfaces;

//namespace WebApi.Controllers
//{
//    [Route("api/store")]
//    [ApiController]
//    public class StoreController (
//        IStoreService storeService
//        ) : ControllerBase
//    {

//        [HttpPost("document/analysis")]
//        public IActionResult StoreDocument([FromBody] DocumentAnalysisFields document)
//        {
//            //storeService.StoreDocumentAnalysisResult(document);

//            return Ok();
//        }

//        [HttpPost("embeddings")]
//        public IActionResult StoreChunks([FromBody] List<EmbeddedChunk> chunks)
//        {
//            // Simulate a create operation
//            return Ok();
//        }

//    }
//}
