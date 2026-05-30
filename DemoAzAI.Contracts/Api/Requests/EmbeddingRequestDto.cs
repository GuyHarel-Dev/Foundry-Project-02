using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAzAI.Contracts.Api.Requests
{
    public class EmbeddingRequestDto
    {
        public bool AddEmbeddingsToStore { get; set; } = false;
        public List<string> Chunks { get; set; } = [];

    }
}
