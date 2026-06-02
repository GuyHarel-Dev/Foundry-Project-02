using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAzAI.Domain.Models.DocumentAnalysis
{
    public class DocumentAnalyzed
    {
        public string DocumentType { get; set; } = string.Empty;
       
        public float DocumentTypeConfidence { get; set; }

        public List<DocumentAnalyzedFields> DocumentFields { get; set; } = new();

    }
}
