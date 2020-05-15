using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFile.Memory;
using System.Threading.Tasks;

namespace NFile.AspNet.Tests
{
    [TestClass]
    public class FileSystemProviderTests
    {
        IFileSystem Memory { get; }
        IFileSystemProvider Provider { get; }

        public FileSystemProviderTests()
        {
            this.Memory = new MemoryFileSystem(new MemoryConfiguration());
            this.Provider = new FileSystemProvider(new[] { Memory });
        }

        [TestMethod]
        public async Task LoadsFromObject()
        {
            var file = this.Memory.GetFile("test.txt");
            await file.Create();

            var fileObj = file.ToFileObject();
            var loaded = this.Provider.GetFile(fileObj);

            var exists = await file.Exists();
            Assert.IsTrue(exists);
        }
    }
}
