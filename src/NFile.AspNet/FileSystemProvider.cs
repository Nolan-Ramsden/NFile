using System;
using System.Collections.Generic;
using System.Linq;

namespace NFile
{
    public class FileSystemProvider : IFileSystemProvider
    {
        protected IEnumerable<IFileSystem> Providers { get; }

        public FileSystemProvider(IEnumerable<IFileSystem> providers)
        {
            this.Providers = providers;
        }

        public IDirectory GetDirectory(DirectoryObject dir)
        {
            return this.GetProvider(dir.Provider).GetDirectory(dir.Path);
        }

        public IFile GetFile(FileObject file)
        {
            return this.GetProvider(file.Provider).GetFile(file.Path);
        }

        public IFileSystemItem GetItem(FileSystemObject obj)
        {
            var provider = this.GetProvider(obj.Provider);
            switch (obj.ItemType)
            {
                case FileSystemItemType.File:
                    return provider.GetFile(obj.Path);
                case FileSystemItemType.Directory:
                    return provider.GetDirectory(obj.Path);
                default:
                    throw new ArgumentException($"Unknown item type");
            }
        }

        public IFileSystem GetProvider(string provider)
        {
            var impl = this.Providers.FirstOrDefault(p => p.Provider == provider);
            if (impl == null)
            {
                throw new ArgumentException($"FileSystem provider {provider} not registered");
            }
            return impl;
        }
    }
}
