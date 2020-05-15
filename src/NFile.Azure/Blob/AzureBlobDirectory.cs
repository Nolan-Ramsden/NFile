using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFile.Azure.Blob
{
    class AzureBlobDirectory : IDirectory
    {
        public FileSystemItemType ItemType => FileSystemItemType.Directory;

        public string Name { get; }
        public string Path { get; }
        public string Provider => AzureBlobFileSystem.ProviderName;

        protected BlobServiceClient Client { get; }

        public AzureBlobDirectory(BlobServiceClient client, string path)
        {
            this.Path = path;
            this.Client = client;
        }

        public Task Create()
        {
            throw new NotImplementedException();
        }

        public Task Delete()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IFileSystemItem>> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IDirectory GetDirectory(string relativePath)
        {
            throw new NotImplementedException();
        }

        public IFile GetFile(string relativePath)
        {
            throw new NotImplementedException();
        }
    }
}
