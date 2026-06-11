namespace DemoAzAI.Infrastructure.ExternalServices.Stores
{
    public class FileStoreService: IFileStoreService
    {
        private readonly string fileStorePath;
        public FileStoreService(string path)
        {
            fileStorePath = path;
        }

        public Task<IEnumerable<string>> GetAllDocuments()
        {
            var docList = Directory.EnumerateFiles(fileStorePath);

            return Task.FromResult(docList);

        }
    }
}
