using System.IO;
using System.Threading.Tasks;

namespace NFile.Providers.Disk
{
    class DiskFileHandle : IFileHandle
    {
        protected FileInfo File { get; set; }
        protected bool BufferStale { get; set; } = false;
        protected bool AppendOnWrite { get; set; } = false;
        protected string Buffer { get; set; } = string.Empty;

        public DiskFileHandle(FileInfo file)
        {
            this.File = file;
        }

        public void Append(string txt)
        {
            this.Buffer += txt;
            this.AppendOnWrite = true;
        }

        public Task<string> Read()
        {
            return System.IO.File.ReadAllTextAsync(
                path: this.File.FullName
            );
        }

        public void Write(string txt)
        {
            this.Buffer = txt;
            this.AppendOnWrite = false;
        }

        public void Clear()
        {
            this.Buffer = string.Empty;
            this.AppendOnWrite = false;
        }

        public async Task Flush()
        {
            if (!this.AppendOnWrite)
            {
                await System.IO.File.WriteAllTextAsync(
                    path: this.File.FullName, 
                    contents: this.Buffer
                );
            }
            if (this.Buffer != string.Empty)
            {
                await System.IO.File.AppendAllTextAsync(
                    path: this.File.FullName,
                    contents: this.Buffer
                );
            }
            this.Buffer = string.Empty;
            this.AppendOnWrite = false;
        }

        public void Dispose()
        {
        }
    }
}
