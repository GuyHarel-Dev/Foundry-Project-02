using DemoAzAI.Domain.Models;

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

        public List<DocumentInfo> GetDocumentFileInfos()
        {
            var files = Directory.EnumerateFiles(fileStorePath);
            var docInfos = new List<DocumentInfo> ();

            foreach (var path in files) 
            {
                docInfos.Add(new DocumentInfo 
                { 
                    FileName = Path.GetFileName(path), 
                    FileUrl = path 
                });
            }


            return docInfos;

        }
    }
}
