using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAzAI.Domain
{
    public record ModelStatistics
    (
        string Model,
        int? InputTokens,
        int? OutputTokens,
        int? TotalTokens,
        int AnswerLength
    );
}
