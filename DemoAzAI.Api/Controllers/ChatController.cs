using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using Microsoft.AspNetCore.Mvc;


namespace DemoAzAI.Api.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController(
        IChatService chatService
        ) : ControllerBase
    {

        [HttpPost("questions/answer")]
        public async Task<ActionResult<ChatQuestionResponseDto>> AnswerQuestion([FromBody] ChatQuestionRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest("Prompt text cannot be empty.");
            }
            var response = await chatService.GetPromptResponseAsync(request);

            return Ok(response);
        }

        [HttpPost("chunkings/chunk")]
        public async Task<ActionResult<DocumentChunkingResponseDto>> ChunkText([FromBody] DocumentChunkingRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest("Prompt text cannot be empty.");
            }
            var response = await chatService.GetDocumentChunkingResponseAsync(request);

            return Ok(response);
        }

    }
}


