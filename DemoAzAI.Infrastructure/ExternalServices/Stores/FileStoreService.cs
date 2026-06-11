using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Infrastructure.ExternalServices.Stores
{
    public class FileStoreService: IFileStoreService
    {
        private readonly string fileStorePath;
        public FileStoreService(string path)
        {
            fileStorePath = path;
        }

        public async Task<List<StoreDocument>> GetAllDocuments()
        {
            var files = Directory.EnumerateFiles(fileStorePath);

            var documents = new List<StoreDocument>();

            foreach (var path in files) 
            {
                documents.Add(new StoreDocument { FilePath = path, FileName = Path.GetFileName(path) });
            }


            return documents;

        }
    }
}
