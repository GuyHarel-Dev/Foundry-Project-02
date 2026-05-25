using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAzAI.Contracts.Responses
{
    public class EmbeddingTextLinesResponseDto
    {
        public List<string> EmbeddedTextLines { get; set; } = new List<string>();
        public List<Guid> EmbeddedTextLinesGuids { get; set; } = new List<Guid>();
        public string EmbeddedTextLinesStats { get; set; } = string.Empty;
    }
}
