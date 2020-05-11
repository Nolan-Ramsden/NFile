using System;
using System.IO;
using System.Threading.Tasks;

namespace NFile.Disk
{
    abstract class DiskItem : IFileSystemItem
    {
        public abstract FileSystemItemType ItemType { get; }
        public string Name => this.Item.Name;
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
            var relativePath = path.Substring(rootPath.Length, path.Length - rootPath.Length);
            return relativePath.Trim(System.IO.Path.DirectorySeparatorChar);
        }

        public abstract Task Create();
        public abstract Task Delete();
    }
}
