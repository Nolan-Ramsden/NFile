using System.IO;
using System.Threading.Tasks;

namespace NFile.Disk
{
    class DiskFileHandle : IFileHandle
    {
        protected FileInfo File { get; set; }
        protected bool BeenWritten { get; set; } = false;
        protected string Buffer { get; set; } = string.Empty;

        public DiskFileHandle(FileInfo file)
        {
            this.File = file;
        }

        public void Append(string txt)
        {
            this.Buffer += txt;
        }

        public void Write(string txt)
        {
            this.Buffer = txt;
            this.BeenWritten = true;
        }

        public Task<string> Read()
        {
            return System.IO.File.ReadAllTextAsync(
                path: this.File.FullName
            );
        }

        public void Clear()
        {
            this.Write(string.Empty);
        }

        public async Task Flush()
        {
            if (this.BeenWritten)
            {
                await System.IO.File.WriteAllTextAsync(
                    path: this.File.FullName, 
                    contents: this.Buffer
                );
            }
            else if (this.Buffer != string.Empty)
            {
                await System.IO.File.AppendAllTextAsync(
                    path: this.File.FullName,
                    contents: this.Buffer
                );
            }
            this.Buffer = string.Empty;
            this.BeenWritten = false;
        }

        public void Dispose()
        {
        }
    }
}
