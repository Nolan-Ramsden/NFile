using System;
using System.IO;
using System.Threading.Tasks;

namespace NFile.Memory
{
    abstract class MemoryItem : IFileSystemItem
    {
        public abstract FileSystemItemType ItemType { get; }
        public string Name { get; }
        public string Path => this.GetPath();
        public string Provider => MemoryFileSystem.ProviderName;

        private bool Created { get; set; }
        private MemoryDirectory Parent { get; }

        public MemoryItem(MemoryDirectory parent, string name)
        {
            this.Name = name;
            this.Parent = parent;
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

        private string GetPath()
        {
            var parentPath = this.Parent?.Path ?? string.Empty;
            var fullPath = System.IO.Path.Combine(parentPath, this.Name);
            return fullPath.Trim(System.IO.Path.DirectorySeparatorChar);
        }
    }
}
