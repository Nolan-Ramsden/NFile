using Azure.Storage.Blobs;
using System;

namespace NFile.Azure.Blob
{
    public class AzureBlobFileSystem : IFileSystem
    {
        public const string ProviderName = "AzureBlob";

        public string Provider => ProviderName;

        protected BlobServiceClient Client { get; }

        public AzureBlobFileSystem(AzureBlobConfiguration config)
        {
            this.Client = new BlobServiceClient(config.ConnectionString);
        }

        public IDirectory GetDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public IFile GetFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}
