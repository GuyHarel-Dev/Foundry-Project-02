using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAzAI.Common
{
    public class StepTracerStep
    {
        public string StepTitle { get; set; } = string.Empty;
        public DateTime? StepDateTime { get; set; }
        public string StepSource { get; set; } = string.Empty;
    }
}
