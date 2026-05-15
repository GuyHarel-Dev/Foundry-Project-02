using AzureAI.Api.Services.Interfaces;
using AzureAI.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AzureAI.Api.Controllers 
{
    [Route("api/ai/doc")]
    [ApiController]
    public class DocAiController : ControllerBase
    {
        private readonly IAzAiDocIntelligenceService _documentIntelligence;

        public DocAiController(IAzAiDocIntelligenceService documentIntelligence)
        {
            _documentIntelligence = documentIntelligence;
        }

        [HttpPost("prebuildmodel/Test")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> PrebuildModelTest(IFormFile formFile)
        {
            var response = await _documentIntelligence.PrebuildModelTest(formFile);

            return Ok(response);

        }

        [HttpPost("prebuildmodel/read")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> PrebuildModelRead()
        {
            var response = await _documentIntelligence.AnalyzeDocument();

            return Ok(response);

        }

        [HttpPost("prebuildmodel/layout")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> PrebuildModelLayout()
        {
            var response = await _documentIntelligence.AnalyzeDocument();

            return Ok(response);

        }

        [HttpPost("prebuildmodel/invoice")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> PrebuildModelInvoice()
        {
            var response = await _documentIntelligence.AnalyzeDocument();

            return Ok(response);

        }

        [HttpPost("prebuildmodel/receipt")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> PrebuildModelReceipt()
        {
            var response = await _documentIntelligence.AnalyzeDocument();

            return Ok(response);

        }

    }
}
