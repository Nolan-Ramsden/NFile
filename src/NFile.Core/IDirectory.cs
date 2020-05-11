using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFile
{
    public interface IDirectory : IFileSystemItem
    {
        Task<IFile> GetFile(string relativePath);

        Task<IDirectory> GetDirectory(string relativePath);

        Task<IEnumerable<IFileSystemItem>> GetChildren();
    }
}
