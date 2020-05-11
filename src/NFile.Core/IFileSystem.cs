using System.Threading.Tasks;

namespace NFile
{
    public interface IFileSystem
    {
        string Provider { get; }

        Task<IFile> GetFile(string path);

        Task<IDirectory> GetDirectory(string path);
    }
}
