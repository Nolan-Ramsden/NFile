using System.Threading.Tasks;

namespace NFile.Memory
{
    public class MemoryFileSystem : IFileSystem
    {
        public const string ProviderName = "Memory";

        public string Provider => ProviderName;

        private MemoryDirectory Root { get; }

        public MemoryFileSystem()
        {
            this.Root = new MemoryDirectory(null, string.Empty);
            this.Root.Create().Wait();
        }

        public Task<IDirectory> GetDirectory(string path)
        {
            return this.Root.GetDirectory(path);
        }

        public Task<IFile> GetFile(string path)
        {
            return this.Root.GetFile(path);
        }
    }
}
