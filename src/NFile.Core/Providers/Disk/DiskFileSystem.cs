using System.IO;
using System.Threading.Tasks;

namespace NFile.Disk
{
    public class DiskFileSystem : IFileSystem
    {
        public const string ProviderName = "Disk";

        public string Provider => ProviderName;

        private DirectoryInfo Root { get; }

        public DiskFileSystem(DirectoryInfo root)
        {
            this.Root = root;
        }

        public Task<IDirectory> GetDirectory(string path)
        {
            var directory = new DirectoryInfo(Path.Combine(this.Root.FullName, path));
            return Task.FromResult<IDirectory>(new DiskDirectory(this.Root, directory));
        }

        public Task<IFile> GetFile(string path)
        {
            var file = new FileInfo(Path.Combine(this.Root.FullName, path));
            return Task.FromResult<IFile>(new DiskFile(this.Root, file));
        }
    }
}
