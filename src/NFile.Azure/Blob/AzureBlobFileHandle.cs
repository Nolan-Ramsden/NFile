using Azure;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace NFile.Azure.Blob
{
    class AzureBlobFileHandle : IFileHandle
    {
        protected BlobClient Client { get; }
        protected bool BeenWritten { get; set; } = false;
        protected string Buffer { get; set; } = string.Empty;

        public AzureBlobFileHandle(BlobClient client)
        {
            this.Client = client;
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

        public async Task<string> Read()
        {
            using (var stream = new MemoryStream())
            {
                try
                {
                    var result = await this.Client.DownloadToAsync(stream);
                }
                catch (RequestFailedException e) when (e.ErrorCode == "InvalidRange")
                {
                    return string.Empty;
                }

                stream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        public void Clear()
        {
            this.Write(string.Empty);
        }

        public async Task Flush()
        {
            if (this.BeenWritten)
            {
                await this.WriteToBlob(this.Buffer);
            }
            else if (this.Buffer != string.Empty)
            {
                var existingContent = await this.Read();
                await this.WriteToBlob(existingContent + this.Buffer);
            }
            this.Buffer = string.Empty;
            this.BeenWritten = false;
        }

        public void Dispose()
        {

        }

        protected async Task WriteToBlob(string txt)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    await writer.WriteAsync(txt);
                    await writer.FlushAsync();
                    stream.Seek(0, SeekOrigin.Begin);
             
                    await this.Client.UploadAsync(stream, overwrite: true);
                }
            }
        }
    }
}
