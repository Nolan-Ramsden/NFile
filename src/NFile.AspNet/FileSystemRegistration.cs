using Microsoft.Extensions.DependencyInjection;
using NFile.Disk;
using NFile.Memory;
using System.IO;

namespace NFile
{
    public static class FileSystemRegistration
    {
        public static IServiceCollection AddNFile(this IServiceCollection services)
        {
            return services.AddTransient<IFileSystemProvider, FileSystemProvider>();
        }

        public static IServiceCollection AddMemoryFileSystem(this IServiceCollection services)
        {
            return services.AddSingleton<IFileSystem, MemoryFileSystem>();
        }

        public static IServiceCollection AddDiskFileSystem(this IServiceCollection services, string root)
        {
            return services.AddTransient<IFileSystem, DiskFileSystem>(
                (services) => new DiskFileSystem(new DirectoryInfo(root))
            );
        }
    }
}
