using System.IO;

namespace NFile.Disk
{
    public class DiskFileSystem : IFileSystem
    {
        public const string ProviderName = "Disk";

        public string Provider => ProviderName;

        protected DirectoryInfo Root { get; }

        public DiskFileSystem(DiskConfiguration config)
        {
            this.Root = new DirectoryInfo(config.Root);
        }

        public IDirectory GetDirectory(string path)
        {
            var directory = new DirectoryInfo(Path.Combine(this.Root.FullName, path));
            return new DiskDirectory(this.Root, directory);
        }

        public IFile GetFile(string path)
        {
            var file = new FileInfo(Path.Combine(this.Root.FullName, path));
            return new DiskFile(this.Root, file);
        }
    }
}
