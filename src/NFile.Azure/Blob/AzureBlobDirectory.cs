using Azure.Storage.Blobs;
using NFile.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NFile.Azure.Blob
{
    class AzureBlobDirectory : AzureBlobItem, IDirectory
    {
        public override FileSystemItemType ItemType => FileSystemItemType.Directory;

        protected AzureBlobFile ExistanceMarker { get; }

        public AzureBlobDirectory(BlobServiceClient client, string path) : base(client, path)
        {
            this.ExistanceMarker = new AzureBlobFile(client, PathUtils.Combine(this.Path, ".marker"));
        }

        public override async Task Create()
        {
            if (string.IsNullOrWhiteSpace(this.ContainerPath))
            {
                var result = await this.ContainerClient.CreateAsync();
                var response = result.GetRawResponse();
                if (response.Status != (int)HttpStatusCode.Created)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

            await this.ExistanceMarker.Create();
        }

        public override async Task Delete()
        {
            if (string.IsNullOrWhiteSpace(this.ContainerPath))
            {
                await this.ContainerClient.DeleteAsync();
                return;
            }

            var children = await this.GetChildren();
            await Task.WhenAll(children.Select(c => c.Delete()));
            await this.ExistanceMarker.Delete();
        }

        public override Task<bool> Exists()
        {
            return this.ExistanceMarker.Exists();
        }

        public async Task<IEnumerable<IFileSystemItem>> GetChildren()
        {
            var azurePath = this.ContainerPath.Replace(PathUtils.Separator, '/');
            var results = this.ContainerClient.GetBlobsByHierarchyAsync(
                prefix: azurePath + "/",
                delimiter: "/"
            );

            var children = new List<IFileSystemItem>();
            await foreach(var result in results)
            {
                if (result.Prefix == null)
                {
                    var fullPath = PathUtils.Combine(this.Container, result.Blob.Name);
                    var item = new AzureBlobFile(this.Client, fullPath);
                    if (item.Path != this.ExistanceMarker.Path)
                    {
                        children.Add(item);
                    }
                }
                else
                {
                    var fullPath = PathUtils.Combine(this.Container, result.Prefix);
                    var item = new AzureBlobDirectory(this.Client, fullPath);
                    children.Add(item);
                }
            }

            return children;
        }

        public IDirectory GetDirectory(string relativePath)
        {
            var fullPath = PathUtils.Combine(this.Path, relativePath);
            return new AzureBlobDirectory(this.Client, fullPath);
        }

        public IFile GetFile(string relativePath)
        {
            var fullPath = PathUtils.Combine(this.Path, relativePath);
            return new AzureBlobFile(this.Client, fullPath);
        }
    }
}
