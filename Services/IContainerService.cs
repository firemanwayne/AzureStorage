using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureStorage.Options;
using static System.Console;

namespace AzureStorage.Services
{
    internal interface IContainerService
    {
        Task<BlobContainerClient> CreateContainer(string container);
        Task DeleteContainer(string container);
        IAsyncEnumerable<BlobContainerItem> GetContainers();
    }

    internal class ContainerService : IContainerService
    {
        readonly BlobServiceClient serviceClient;

        public ContainerService(BlobStorageOptions opts)
        {
            serviceClient = new(opts.ConnectionString);
        }
        
        public async Task<BlobContainerClient> CreateContainer(string container)
        {
            var response = await serviceClient.CreateBlobContainerAsync(container, PublicAccessType.BlobContainer);
            var rawResponse = response.GetRawResponse();

            WriteLine($"Azure Storage HttpStatus: {rawResponse.Status}");

            return response.Value;
        }

        public Task DeleteContainer(string container) => serviceClient.DeleteBlobContainerAsync(container);

        public IAsyncEnumerable<BlobContainerItem> GetContainers() => serviceClient.GetBlobContainersAsync();
    }
}
