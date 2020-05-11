using System.Threading.Tasks;

namespace NFile
{
    public interface IFileSystemItem
    {
        FileSystemItemType ItemType { get; }

        string Name { get; }

        string Path { get; }

        string Provider { get; }

        Task Create();

        Task Delete();

        Task<bool> Exists();
    }
}
