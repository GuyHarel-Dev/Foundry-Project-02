using System.Text;

namespace Shared.DTOs
{
    public sealed class ModelStatisticsDto
    {
        public string ResponseId { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int? InputTokens { get; set; }
        public int? OutputTokens { get; set; }
        public int? TotalTokens { get; set; }
        public int AnswerLength { get; set; }
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"ResponseId: {ResponseId}");
            builder.AppendLine($"Model: {Model}");
            builder.AppendLine($"InputTokens: {InputTokens}");
            builder.AppendLine($"OutputTokens: {OutputTokens}");
            builder.AppendLine($"TotalTokens: {TotalTokens}");
            builder.AppendLine($"AnswerLength: {AnswerLength}");

            return builder.ToString();
        }
    }

}
