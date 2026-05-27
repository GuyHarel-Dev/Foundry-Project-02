using DemoAzAI.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAzAI.Contracts.Responses
{
    public class EmbeddingTextResponseDto
    {
        public List<TextEmbeddingDto> TextEmbeddings { get; set; } = new List<TextEmbeddingDto>();
    }
}
