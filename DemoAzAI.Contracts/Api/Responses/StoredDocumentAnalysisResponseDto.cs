using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class StoredDocumentAnalysisResponseDto
    {
        public string FileName { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
