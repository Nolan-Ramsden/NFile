using NFile.Util;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NFile.Memory
{
    class MemoryDirectory : MemoryItem, IDirectory
    {
        public override FileSystemItemType ItemType => FileSystemItemType.Directory;

        protected Dictionary<string, MemoryItem> Children { get; } = new Dictionary<string, MemoryItem>();

        public MemoryDirectory(MemoryDirectory parent, string name) : base(parent, name)
        {
        }

        public Task<IEnumerable<IFileSystemItem>> GetChildren()
        {
            return Task.FromResult<IEnumerable<IFileSystemItem>>(this.Children.Values);
        }

        public IDirectory GetDirectory(string relativePath)
        {
            var treated = PathUtils.Normalize(relativePath);
            var pieces = PathUtils.Split(treated);
            var dirName = pieces.First();

            IDirectory child = null;
            if (this.Children.ContainsKey(dirName))
            {
                var item = this.Children[dirName];
                if (item.ItemType == FileSystemItemType.Directory)
                {
                    child = item as MemoryDirectory;
                }
            }

            child = child ?? new MemoryDirectory(this, dirName);
            if (pieces.Count() == 1)
            {
                return child;
            }

            var relative = PathUtils.Combine(pieces.Skip(1).ToArray());
            return child.GetDirectory(relative);
        }

        public IFile GetFile(string relativePath)
        {
            var treated = PathUtils.Normalize(relativePath);
            var pieces = PathUtils.Split(treated);
            if (pieces.Count() == 1)
            {
                var fileName = PathUtils.GetName(treated);
                if (this.Children.ContainsKey(fileName))
                {
                    var item = this.Children[fileName];
                    if (item.ItemType == FileSystemItemType.File)
                    {
                        return item as MemoryFile;
                    }
                }
                return new MemoryFile(this, fileName);
            }

            var relativeDir = PathUtils.Combine(pieces.SkipLast(1).ToArray());
            var childDir = this.GetDirectory(relativeDir);
            return childDir.GetFile(pieces.Last());
        }

        internal void AddChild(MemoryItem item)
        {
            this.Children[item.Name] = item;
        }

        internal void RemoveChild(MemoryItem item)
        {
            this.Children.Remove(item.Name);
        }
    }
}
