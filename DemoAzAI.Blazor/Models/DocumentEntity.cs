namespace DemoAzAI.Blazor.Models
{
    public class DocumentEntity
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public override string ToString()
        {
            return $"{Id}-{Name}-{Name}-{FilePath}";
        }
    }
}
