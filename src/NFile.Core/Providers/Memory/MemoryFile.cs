using System.Threading.Tasks;

namespace NFile.Memory
{
    class MemoryFile : MemoryItem, IFile
    {
        public override FileSystemItemType ItemType => FileSystemItemType.File;

        public string Contents { get; set; } = string.Empty;

        public MemoryFile(MemoryDirectory parent, string name) : base(parent, name)
        {
        }

        public Task<IFileHandle> Open()
        {
            return Task.FromResult<IFileHandle>(new MemoryFileHandle(this));
        }
    }
}
