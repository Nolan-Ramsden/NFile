using System.Threading.Tasks;

namespace NFile
{
    public interface IFile : IFileSystemItem
    {
        IFileHandle Open();
    }
}
