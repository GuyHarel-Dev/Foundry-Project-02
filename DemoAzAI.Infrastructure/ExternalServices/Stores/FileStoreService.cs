using DemoAzAI.Domain.Entities;
using System.Text;

namespace DemoAzAI.Infrastructure.ExternalServices.Stores
{
    public class FileStoreService: IFileStoreService
    {
        private readonly string fileStorePath;
        public FileStoreService(string path)
        {
            fileStorePath = path;
        }

        //public async Task<Stream> GetDocumentStream(StoreDocument document)
        //{

        //}

        public async Task<List<string>> GetAllDocumentNames()
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
