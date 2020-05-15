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

        public static IServiceCollection AddMemoryFileSystem(this IServiceCollection services, MemoryConfiguration config)
        {
            return services.AddSingleton<IFileSystem>(new MemoryFileSystem(config));
        }

        public static IServiceCollection AddDiskFileSystem(this IServiceCollection services, DiskConfiguration config)
        {
            return services.AddTransient<IFileSystem, DiskFileSystem>(
                (services) => new DiskFileSystem(config)
            );
        }
    }
}
