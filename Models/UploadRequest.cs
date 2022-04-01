using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AzureStorage
{
    public class UploadRequest
    {
        public UploadRequest() { }       

        public UploadRequest((string fileName, string container, IBrowserFile file) request)
        {
            FileName = request.fileName;
            ContainerName = request.container;
            Size = request.file.Size;
            Content = new byte[Size];
        }

        public string? ContainerName { get; set; }
        public string? FileName { get; set; }
        public long Size { get; set; }
        public byte[]? Content { get; set; }
        public string? Base64data { get; set; }
        public string? ContentType { get; set; }
    }
}
