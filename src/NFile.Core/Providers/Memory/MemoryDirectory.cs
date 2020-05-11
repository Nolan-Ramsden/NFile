﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NFile.Memory
{
    class MemoryDirectory : MemoryItem, IDirectory
    {
        public override FileSystemItemType ItemType => FileSystemItemType.Directory;

        private Dictionary<string, MemoryItem> Children { get; } = new Dictionary<string, MemoryItem>();

        public MemoryDirectory(MemoryDirectory parent, string name) : base(parent, name)
        {
        }

        public Task<IEnumerable<IFileSystemItem>> GetChildren()
        {
            return Task.FromResult<IEnumerable<IFileSystemItem>>(this.Children.Values);
        }

        public Task<IDirectory> GetDirectory(string relativePath)
        {
            var treated = relativePath.Trim(System.IO.Path.DirectorySeparatorChar);
            var pieces = treated.Split(System.IO.Path.DirectorySeparatorChar);
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
            if (pieces.Length == 1)
            {
                return Task.FromResult(child);
            }

            var relative = System.IO.Path.Combine(pieces.Skip(1).ToArray());
            return child.GetDirectory(relative);
        }

        public async Task<IFile> GetFile(string relativePath)
        {
            var treated = relativePath.Trim(System.IO.Path.DirectorySeparatorChar);
            var pieces = treated.Split(System.IO.Path.DirectorySeparatorChar);
            if (pieces.Length == 1)
            {
                var fileName = pieces.First();
                if (this.Children.ContainsKey(fileName))
                {
                    var item = this.Children[pieces.First()];
                    if (item.ItemType == FileSystemItemType.File)
                    {
                        return item as MemoryFile;
                    }
                }
                return new MemoryFile(this, pieces.First());
            }

            var relativeDir = System.IO.Path.Combine(pieces.SkipLast(1).ToArray());
            var childDir = await this.GetDirectory(relativeDir);
            return await childDir.GetFile(pieces.Last());
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