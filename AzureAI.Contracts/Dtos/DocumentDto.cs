using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAI.Contracts.Dtos
{
    public class DocumentDto
    {
        public string? DocumentType { get; set; }

        public float? Confidence { get; set; }

        public List<DocumentFieldDto> Fields { get; set; } = [];
    }
}
