using System;
using System.Threading.Tasks;

namespace NFile
{
    public interface IFileHandle : IDisposable
    {
        void Clear();

        void Write(string txt);

        void Append(string txt);

        Task<string> Read();

        Task Flush();
    }
}
