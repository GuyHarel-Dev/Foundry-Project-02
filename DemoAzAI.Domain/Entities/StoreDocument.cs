namespace DemoAzAI.Domain.Entities
{
    public class StoreDocument
    {
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public byte[] Bytes { get; set; } = Array.Empty<byte>();
        public string BytesHash { get; set; } = string.Empty;
        public override string ToString()
        {
            return $"{FileName}";
        }
    }
}
