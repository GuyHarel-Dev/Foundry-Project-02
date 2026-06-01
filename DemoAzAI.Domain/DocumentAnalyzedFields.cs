namespace DemoAzAI.Domain
{
    public class DocumentAnalyzedFields
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string FieldType { get; set; } = string.Empty;
        public float? FieldTypeConfidence { get; set; }
    };

}
