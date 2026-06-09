using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAzAI.Contracts.Api.Requests
{
    public class EmbeddingRequestDto
    {
        public string Text { get; set; } = string.Empty;
        public List<string> TextLines { get; set; } = new();

    }
}
