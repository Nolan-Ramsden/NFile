using Azure.Storage.Blobs;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NFile.Azure.Blob
{
    class AzureBlobFile : AzureBlobItem, IFile
    {
        public override FileSystemItemType ItemType => FileSystemItemType.File;

        protected BlobClient BlobClient { get; }

        public AzureBlobFile(BlobServiceClient client, string path) : base(client, path)
        {
            BlobClient = this.ContainerClient.GetBlobClient(this.ContainerPath);
        }

        public override async Task Create()
        {
            if (string.IsNullOrWhiteSpace(this.ContainerPath))
            {
                throw new NotImplementedException("Top-level files not allowed in " + this.Provider);
            }
            using (var handle = this.Open())
            {
                handle.Write(string.Empty);
                await handle.Flush();
            }
        }

        public override Task Delete()
        {
            return this.BlobClient.DeleteAsync();
        }

        public override async Task<bool> Exists()
        {
            var result = await this.BlobClient.ExistsAsync();
            return result.Value;
        }

        public IFileHandle Open()
        {
            return new AzureBlobFileHandle(this.BlobClient);
        }
    }
}
