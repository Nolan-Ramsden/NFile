using Azure.Storage.Blobs;
using NFile.Util;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NFile.Azure.Blob
{
    abstract class AzureBlobItem : IFileSystemItem
    {
        public abstract FileSystemItemType ItemType { get; }

        public string Name { get; }
        public string Path { get; }
        public string Provider => AzureBlobFileSystem.ProviderName;

        protected string Container { get; }
        protected string ContainerPath { get; }
        protected BlobServiceClient Client { get; }
        protected BlobContainerClient ContainerClient { get; }

        public AzureBlobItem(BlobServiceClient client, string path)
        {
            this.Client = client;
            this.Name = PathUtils.GetName(path);
            this.Path = PathUtils.Normalize(path);
            this.Container = GetContainer(path);
            this.ContainerPath = GetContainerPath(path);
            this.ContainerClient = Client.GetBlobContainerClient(this.Container);
        }
        private string GetContainer(string path)
        {
            var normal = PathUtils.Normalize(path);
            var pieces = PathUtils.Split(normal);
            return pieces.First();
        }

        private string GetContainerPath(string path)
        {
            var normal = PathUtils.Normalize(path);
            var pieces = PathUtils.Split(normal);
            return PathUtils.Combine(pieces.Skip(1).ToArray());
        }

        public abstract Task Create();

        public abstract Task Delete();

        public abstract Task<bool> Exists();
    }
}
