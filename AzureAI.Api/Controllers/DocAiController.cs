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

        [HttpGet("prebuildmodel/read")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> PrebuildModelRead()
        {
            var response = await _documentIntelligence.AnalyzeDocument();

            return Ok(response);

        }

        [HttpGet("prebuildmodel/layout")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> PrebuildModelLayout()
        {
            var response = await _documentIntelligence.AnalyzeDocument();

            return Ok(response);

        }

        [HttpGet("prebuildmodel/invoice")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> PrebuildModelInvoice()
        {
            var response = await _documentIntelligence.AnalyzeDocument();

            return Ok(response);

        }

        [HttpGet("prebuildmodel/receipt")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> PrebuildModelReceipt()
        {
            var response = await _documentIntelligence.AnalyzeDocument();

            return Ok(response);

        }

    }
}
