using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAI.Contracts.Dtos
{
    public class DocumentFieldDto
    {
        public string? Name { get; set; }

        public string? Value { get; set; }

        public string? FieldType { get; set; }

        public float? Confidence { get; set; }
    }
}
