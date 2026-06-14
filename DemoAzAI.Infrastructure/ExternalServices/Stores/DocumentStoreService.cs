namespace DemoAzAI.Infrastructure.ExternalServices.Stores
{
    public class DocumentStoreService: IDocumentStoreService
    {
        private readonly string fileStorePath;
        public DocumentStoreService(string path)
        {
            fileStorePath = path;
        }

        public Stream GetDocumentFileStream(string fileName)
        {
            return File.OpenRead($"{fileStorePath}/{fileName}");
        }

        public List<string> GetDocumentFileNames()
        {
            var files = Directory.EnumerateFiles(fileStorePath);
            var fileNames = new List<string> ();

            foreach (var path in files) 
            {
                fileNames.Add(Path.GetFileName(path));
            }


            return fileNames;

        }
    }
}
