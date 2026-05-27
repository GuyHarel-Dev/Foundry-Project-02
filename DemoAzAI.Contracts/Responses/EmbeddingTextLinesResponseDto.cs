using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAzAI.Contracts.Responses
{
    public class EmbeddingTextLinesResponseDto
    {
        public List<string> TextLines { get; set; } = new List<string>();
        public List<Guid> Guid { get; set; } = new List<Guid>();
        public string FloatValuesPreview { get; set; } = string.Empty;
    }
}
