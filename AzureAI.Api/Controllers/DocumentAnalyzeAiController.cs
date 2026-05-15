using AzureAI.Api.Services.Interfaces;
using AzureAI.Contracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureAI.Api.Controllers 
{
    [Route("api/document")]
    [ApiController]
    public class DocumentAnalyzeAiController : ControllerBase
    {
        private readonly IAzAiDocIntelligenceService _documentIntelligence;

        public DocumentAnalyzeAiController(IAzAiDocIntelligenceService documentIntelligence)
        {
            _documentIntelligence = documentIntelligence;
        }

        [HttpPost("analyze/Test")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> PrebuildModelTest(IFormFile formFile)
        {
            var response = await _documentIntelligence.PrebuildModelTest(formFile);

            return Ok(response);

        }

        [HttpPost("analyze/read")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> PrebuildModelRead(IFormFile formFile)
        {
            var response = await _documentIntelligence.AnalyzeDocument("prebuilt-read", formFile);

            return Ok(response);

        }

        [HttpPost("analyze/layout")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> PrebuildModelLayout(IFormFile formFile)
        {
            var response = await _documentIntelligence.AnalyzeDocument("prebuilt-layout", formFile);

            return Ok(response);

        }

        [HttpPost("analyze/invoice")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> PrebuildModelInvoice(IFormFile formFile)
        {
            var response = await _documentIntelligence.AnalyzeDocument("prebuilt-invoice", formFile);

            return Ok(response);

        }

        [HttpPost("analyze/receipt")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> PrebuildModelReceipt(IFormFile formFile)
        {
            var response = await _documentIntelligence.AnalyzeDocument("prebuilt-receipt", formFile);

            return Ok(response);

        }

    }
}
