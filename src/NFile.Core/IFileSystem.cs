using System.Threading.Tasks;

namespace NFile
{
    public interface IFileSystem
    {
        string Provider { get; }

        IFile GetFile(string path);

        IDirectory GetDirectory(string path);
    }
}
