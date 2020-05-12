using System.Threading.Tasks;

namespace NFile.Memory
{
    class MemoryFileHandle : IFileHandle
    {
        protected MemoryFile File { get; }
        protected bool BeenWritten { get; set; } = false;
        protected string Buffer { get; set; } = string.Empty;

        public MemoryFileHandle(MemoryFile file)
        {
            this.File = file;
        }

        public void Write(string txt)
        {
            this.Buffer = txt;
            this.BeenWritten = true;
        }

        public void Append(string txt)
        {
            this.Buffer += txt;
        }

        public void Clear()
        {
            this.Write(string.Empty);
        }

        public Task<string> Read()
        {
            return Task.FromResult(this.File.Contents);
        }

        public Task Flush()
        {
            if (this.BeenWritten)
            {
                this.File.Contents = Buffer;
            }
            else
            {
                this.File.Contents += Buffer;
            }
            this.BeenWritten = false;
            this.Buffer = string.Empty;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            
        }
    }
}
