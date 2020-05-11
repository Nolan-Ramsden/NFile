using System.Threading.Tasks;

namespace NFile.Memory
{
    class MemoryFileHandle : IFileHandle
    {
        protected MemoryFile File { get; }
        protected string Buffer { get; set; }

        public MemoryFileHandle(MemoryFile file)
        {
            this.File = file;
            this.Buffer = file.Contents;
        }

        public void Write(string txt)
        {
            this.Buffer = txt;
        }

        public void Append(string txt)
        {
            this.Buffer += txt;
        }

        public void Clear()
        {
            this.Buffer = string.Empty;
        }

        public Task<string> Read()
        {
            return Task.FromResult(this.Buffer);
        }

        public Task Flush()
        {
            this.File.Contents = Buffer;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            
        }
    }
}
