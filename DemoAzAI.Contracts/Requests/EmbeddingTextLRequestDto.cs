using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAzAI.Contracts.Requests
{
    public class EmbeddingTextRequestDto
    {
        public List<string> TextLines { get; set; } = [];

    }
}
