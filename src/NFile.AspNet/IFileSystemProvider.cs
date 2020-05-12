namespace NFile
{
    public interface IFileSystemProvider
    {
        IFile GetFile(FileObject file);
        
        IDirectory GetDirectory(DirectoryObject dir);
        
        IFileSystemItem GetItem(FileSystemObject obj);

        IFileSystem GetProvider(string provider);
    }
}
