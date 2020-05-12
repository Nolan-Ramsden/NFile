using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFile
{
    public interface IDirectory : IFileSystemItem
    {
        IFile GetFile(string relativePath);

        IDirectory GetDirectory(string relativePath);

        Task<IEnumerable<IFileSystemItem>> GetChildren();
    }
}
