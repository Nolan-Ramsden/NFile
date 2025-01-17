﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFile.Disk;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NFile.Core.Tests
{
    [TestClass]
    public class DiskFileSystemTests
    {
        static string TestRoot { get; } = @"C:\Users\nolan\Downloads\scratch";

        DiskFileSystem Fs { get; } = new DiskFileSystem(new DiskConfiguration() { Root = TestRoot });

        [TestMethod]
        public async Task CreateDirectory()
        {
            var dir = this.Fs.GetDirectory(@"test_directory\");
            Assert.AreEqual("test_directory", dir.Name);
            Assert.AreEqual("test_directory", dir.Path);

            var exists = await dir.Exists();
            Assert.IsFalse(exists);

            await dir.Create();
            var existsAfter = await dir.Exists();
            Assert.IsTrue(existsAfter);

            await dir.Delete();
            var existsFinal = await dir.Exists();
            Assert.IsFalse(existsFinal);
        }

        [TestMethod]
        public async Task CreateFile()
        {
            var file = this.Fs.GetFile("test_file.txt");
            Assert.AreEqual("test_file.txt", file.Name);
            Assert.AreEqual("test_file.txt", file.Path);

            var exists = await file.Exists();
            Assert.IsFalse(exists);

            await file.Create();
            var existsAfter = await file.Exists();
            Assert.IsTrue(existsAfter);

            await file.Delete();
            var existsFinal = await file.Exists();
            Assert.IsFalse(existsFinal);
        }

        [TestMethod]
        public async Task CreateFileInSubdir()
        {
            var dir1 = this.Fs.GetDirectory("dir1");
            await dir1.Create();

            var dir2 = dir1.GetDirectory("dir2");
            Assert.AreEqual(@"dir1\dir2", dir2.Path);
            await dir2.Create();

            var file = dir2.GetFile("test_file.txt");
            Assert.AreEqual(@"dir1\dir2\test_file.txt", file.Path);
            await file.Create();

            var dir2FromRoot = this.Fs.GetDirectory(@"dir1\dir2");
            var dir2Exists = await dir2FromRoot.Exists();
            Assert.IsTrue(dir2Exists);

            var fileFromRoot = this.Fs.GetFile(@"dir1\dir2\test_file.txt");
            var fileExists = await fileFromRoot.Exists();
            Assert.IsTrue(fileExists);
        }

        [TestMethod]
        public async Task DeleteDirectoryWithItems()
        {
            var dir1 = this.Fs.GetDirectory("dir1");
            await dir1.Create();

            var dir2 = dir1.GetDirectory("dir2");
            await dir2.Create();

            var file = dir2.GetFile("test_file.txt");
            await file.Create();

            await dir2.Delete();

            var dir1Exists = await dir1.Exists();
            Assert.IsTrue(dir1Exists);

            var dir2Exists = await dir2.Exists();
            Assert.IsFalse(dir2Exists);

            var fileExists = await file.Exists();
            Assert.IsFalse(fileExists);
        }

        [TestMethod]
        public async Task EnumerateChildren()
        {
            var dir1 = this.Fs.GetDirectory("dir1");
            await dir1.Create();

            var dir2 = dir1.GetDirectory("dir2");
            await dir2.Create();

            var file1 = dir2.GetFile("file1.txt");
            await file1.Create();

            var file2 = dir2.GetFile("file2.txt");
            await file2.Create();

            var dir1Children = await dir1.GetChildren();
            Assert.AreEqual(1, dir1Children.Count());

            var dir2Children = await dir2.GetChildren();
            Assert.AreEqual(2, dir2Children.Count());
        }

        [TestMethod]
        public async Task WriteReadFile()
        {
            var file = this.Fs.GetFile("test_file.txt");
            await file.Create();

            using (var handle = file.Open())
            {
                var contents = await handle.Read();
                Assert.AreEqual(string.Empty, contents);

                handle.Write("test_1 ");
                handle.Append("test_2 ");
                handle.Write("test_3 ");

                contents = await handle.Read();
                Assert.AreEqual(string.Empty, contents);

                await handle.Flush();
                contents = await handle.Read();
                Assert.AreEqual("test_3 ", contents);

                handle.Append("test_2 ");

                await handle.Flush();
                contents = await handle.Read();
                Assert.AreEqual("test_3 test_2 ", contents);
            }
        }

        [TestMethod]
        public async Task IgnoresPlatform()
        {
            var dirTrail = this.Fs.GetDirectory("testdir/");
            await dirTrail.Create();
            var dirTrailExists = await dirTrail.Exists();
            Assert.IsTrue(dirTrailExists);

            var dir = this.Fs.GetDirectory("testdir");
            var dirExists = await dir.Exists();
            Assert.IsTrue(dirExists);

            var dirTrail2 = this.Fs.GetDirectory(@"testdir\");
            var dirTrail2Exists = await dirTrail2.Exists();
            Assert.IsTrue(dirTrail2Exists);

            var dirLead = this.Fs.GetDirectory(@"\testdir");
            var dirLeadExists = await dirLead.Exists();
            Assert.IsTrue(dirLeadExists);

            var file = dir.GetFile("test_file.txt");
            await file.Create();
            var fileExists = await file.Exists();
            Assert.IsTrue(fileExists);

            var fileMixed = this.Fs.GetFile(@"/testdir\test_file.txt");
            var fileMixedExists = await fileMixed.Exists();
            Assert.IsTrue(fileMixedExists);
        }

        [TestCleanup]
        public void Cleanup()
        {
            var tr = new DirectoryInfo(TestRoot);
            foreach (FileInfo file in tr.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in tr.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
