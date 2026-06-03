using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAzAI.Infrastructure.Persistence
{
    public class DocumentAnalysisEntity
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
        public string AnalysisResult { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
