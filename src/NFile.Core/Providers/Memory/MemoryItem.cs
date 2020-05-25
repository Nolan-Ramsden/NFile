using NFile.Util;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NFile.Memory
{
    abstract class MemoryItem : IFileSystemItem
    {
        public abstract FileSystemItemType ItemType { get; }
        public string Name { get; }
        public string Path { get; }
        public string Provider => MemoryFileSystem.ProviderName;

        protected bool Created { get; set; }
        protected MemoryDirectory Parent { get; }

        public MemoryItem(MemoryDirectory parent, string name)
        {
            this.Parent = parent;
            this.Name = PathUtils.GetName(name);
            this.Path = PathUtils.Combine(parent?.Path ?? string.Empty, name);
        }

        public async Task Create()
        {
            if (this.Parent != null)
            {
                var parentExists = await this.Parent?.Exists();
                if (!parentExists)
                {
                    throw new DirectoryNotFoundException($"Parent directory {this.Parent.Path} not found");
                }
            }
            this.Parent?.AddChild(this);
            this.Created = true;
        }

        public Task Delete()
        {
            this.Created = false;
            this.Parent?.RemoveChild(this);
            return Task.CompletedTask;
        }

        public Task<bool> Exists()
        {
            if (!this.Created)
            {
                return Task.FromResult(false);
            }
            if (this.Parent != null)
            {
                return this.Parent.Exists();
            }
            return Task.FromResult(true);
        }
    }
}
