using System.Threading.Tasks;

namespace NFile
{
    public interface IFile : IFileSystemItem
    {
        Task<IFileHandle> Open();
    }
}
