namespace DemoAzAI.Domain
{
    public record DocumentAnalysisResultField
    (
        string Name,
        string Value,
        string FieldType,
        float Confidence
    );

}
