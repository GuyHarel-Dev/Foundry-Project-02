using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDTO
{
    public sealed class ExecutionTraceStepDto
    {
        public int Order { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public string Status { get; set; } = "Completed";
        public long DurationMs { get; set; }
    }
}
