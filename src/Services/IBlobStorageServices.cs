using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureStorage.Options;
using static System.Console;

namespace AzureStorage.Services
{
    public interface IBlobStorageServices
    {
        Task<BlobClient?> Upload(UploadRequest request);
        Task UploadBlobsInChunks(FileChunk chunk);

        IAsyncEnumerable<BlobItem> GetBlobs(string container);

        Task<BlobClient>? GetBlob((string container, string fileName) b);        
    }

    internal class BlobStorageServices : IBlobStorageServices
    {
        readonly BlobServiceClient serviceClient;

        public BlobStorageServices(BlobStorageOptions opts)
        {
            serviceClient = new(opts.ConnectionString);
        }        

        public Task<BlobClient>? GetBlob((string container, string fileName) b)
        {
            var containerClient = serviceClient.GetBlobContainerClient(b.container);

            if (containerClient != null)
                return Task.FromResult(containerClient.GetBlobClient(b.fileName));

            return default;
        }

        public async IAsyncEnumerable<BlobItem> GetBlobs(string container)
        {
            var containerClient = serviceClient.GetBlobContainerClient(container);

            if (containerClient != null)
                await foreach (var blobItem in containerClient.GetBlobsAsync())
                {
                    WriteLine("\t" + blobItem.Name);
                    yield return blobItem;
                }
        }

        public async Task<BlobClient?> Upload(UploadRequest request) => await serviceClient.UploadAsync(request);

        public async Task UploadBlobsInChunks(FileChunk chunk)
        {
            var containerClient = serviceClient.GetBlobContainerClient(chunk.ContainerName);
            containerClient.CreateIfNotExists(PublicAccessType.BlobContainer);

            var blobClient = containerClient.GetBlobClient(chunk.FileName);

            if (await blobClient.ExistsAsync())
            {
                //blobClient
            }
            else
            {
                if (chunk.Data != null)
                    await blobClient.UploadAsync(new MemoryStream(chunk.Data));
            }
        }
    }

    static class ClientExtensions
    {
        public static async Task<BlobClient?> UploadAsync(this BlobServiceClient client, UploadRequest request)
        {
            try
            {
                var containerClient = client.GetBlobContainerClient(request.ContainerName);

                if (containerClient != null)
                {
                    var response = await containerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

                    if (response != null)
                        await CreateContainer(containerClient, response);

                    var blobClient = containerClient.GetBlobClient(request.FileName);

                    if (request.Base64data != null)
                        await blobClient.UploadAsync(new MemoryStream(Convert.FromBase64String(request.Base64data)), true);

                    return blobClient;
                }
                return default;
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);

                return default;
            }          
        }

        static async Task<Response> CreateContainer(BlobContainerClient containerClient, Response<BlobContainerInfo> response)
        {
            var rawResponse = response.GetRawResponse();

            if (rawResponse.Status == 201)
                await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

            return rawResponse;
        }
    }
}
