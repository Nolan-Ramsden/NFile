namespace NFile
{
    public class FileSystemObject
    {
        public string Provider { get; set; }
        
        public string Path { get; set; }

        public FileSystemItemType ItemType { get; set; }
    }

    public class FileObject : FileSystemObject { }
    
    public class DirectoryObject : FileSystemObject { }

    public static class FileSystemObjectExtensions
    {
        public static FileObject ToFileObject(this IFile file)
        {
            return new FileObject()
            {
                Provider = file.Provider,
                Path = file.Path,
                ItemType = file.ItemType,
            };
        }
        public static DirectoryObject ToDirectoryObject(this IDirectory dir)
        {
            return new DirectoryObject()
            {
                Provider = dir.Provider,
                Path = dir.Path,
                ItemType = dir.ItemType,
            };
        }
        public static FileObject ToObject(this IFileSystemItem item)
        {
            return new FileObject()
            {
                Provider = item.Provider,
                Path = item.Path,
                ItemType = item.ItemType,
            };
        }
    }
}
