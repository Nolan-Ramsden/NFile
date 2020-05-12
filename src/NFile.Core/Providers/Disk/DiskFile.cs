using System.IO;
using System.Threading.Tasks;

namespace NFile.Disk
{
    class DiskFile : DiskItem, IFile
    {
        public override FileSystemItemType ItemType => FileSystemItemType.File;

        protected FileInfo File { get; }

        public DiskFile(DirectoryInfo root, FileInfo file) : base(root, file)
        {
            this.File = file;
        }

        public override Task Create()
        {
            this.File.Refresh();
            this.File.Create().Close();
            return Task.CompletedTask;
        }

        public override Task Delete()
        {
            this.File.Refresh();
            this.File.Delete();
            return Task.CompletedTask;
        }

        public IFileHandle Open()
        {
            return new DiskFileHandle(this.File);
        }
    }
}
