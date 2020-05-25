using NFile.Util;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NFile.Disk
{
    class DiskDirectory : DiskItem, IDirectory
    {
        public override FileSystemItemType ItemType => FileSystemItemType.Directory;

        protected DirectoryInfo Directory { get; }

        public DiskDirectory(DirectoryInfo root, DirectoryInfo directory) : base(root, directory)
        {
            this.Directory = directory;
        }

        public override Task Create()
        {
            this.Directory.Refresh();
            this.Directory.Create();
            return Task.CompletedTask;
        }

        public override Task Delete()
        {
            this.Directory.Refresh();
            this.Directory.Delete(recursive: true);
            return Task.CompletedTask;
        }


        public Task<IEnumerable<IFileSystemItem>> GetChildren()
        {
            var children = new List<IFileSystemItem>();
            children.AddRange(this.Directory.EnumerateDirectories().Select(d => new DiskDirectory(this.Root, d)));
            children.AddRange(this.Directory.EnumerateFiles().Select(d => new DiskFile(this.Root, d)));
            return Task.FromResult<IEnumerable<IFileSystemItem>>(children);
        }

        public IDirectory GetDirectory(string relativePath)
        {
            var normal = PathUtils.Normalize(relativePath);
            var plat = PathUtils.Platformize(normal);
            var fullPath = System.IO.Path.Combine(this.Directory.FullName, plat);
            var directory = new DirectoryInfo(fullPath);
            return new DiskDirectory(this.Root, directory);
        }

        public IFile GetFile(string relativePath)
        {
            var normal = PathUtils.Normalize(relativePath);
            var plat = PathUtils.Platformize(normal);
            var fullPath = System.IO.Path.Combine(this.Directory.FullName, plat);
            var file = new FileInfo(fullPath);
            return new DiskFile(this.Root, file);
        }
    }
}
