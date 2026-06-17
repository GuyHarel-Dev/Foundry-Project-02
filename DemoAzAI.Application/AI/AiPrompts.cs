namespace DemoAzAI.Application.AI
{
    public static class AiPrompts
    {
        public const string DocumentKnowledgeExtraction = """
        You are a text chunking service. 
        You analyse the given prompt and extract the semantically relevant name and value pairs.
        Each name and value pair separated by a colon, on a separe line.
        Just supply the answer, simple text line, no json, no dialog or explanation
        """;
    }
}
