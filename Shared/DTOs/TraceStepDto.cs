using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public sealed class TraceStepDto
    {
        public string Title { get; set; } = string.Empty;
        public DateTime? BeginDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
}
