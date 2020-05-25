using NFile.Util;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NFile.Disk
{
    abstract class DiskItem : IFileSystemItem
    {
        public abstract FileSystemItemType ItemType { get; }
        public string Name => PathUtils.GetName(this.Path);
        public string Path => this.GetPath();
        public string Provider => DiskFileSystem.ProviderName;

        protected DirectoryInfo Root { get; }
        private FileSystemInfo Item { get; }

        public DiskItem(DirectoryInfo root, FileSystemInfo item)
        {
            this.Root = root;
            this.Item = item;
        }

        public virtual Task<bool> Exists()
        {
            this.Item.Refresh();
            return Task.FromResult(this.Item.Exists);
        }

        private string GetPath()
        {
            var path = this.Item.FullName;
            var rootPath = this.Root.FullName;
            if (!path.StartsWith(rootPath))
            {
                throw new ArgumentException("Disk item root misconfigured");
            }
            var relativePath = System.IO.Path.GetRelativePath(rootPath, path);
            return PathUtils.Normalize(relativePath);
        }

        public abstract Task Create();
        public abstract Task Delete();
    }
}
