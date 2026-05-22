using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAzAI.Contracts.Requests
{
    public class EmbeddingRequestDto
    {
        public string Id { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;

    }
}
