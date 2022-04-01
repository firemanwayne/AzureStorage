using Azure.Data.Tables;
using AzureStorage.Options;
using AzureStorage.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace AzureStorage.Extensions
{
    public static class ServiceRegistryExtension
    {
        public static IServiceCollection AddAzureBlobStorage(this IServiceCollection s, Action<BlobStorageOptions> o)
        {
            s.Configure(o);

            s.TryAddScoped<IBlobStorageServices>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<BlobStorageOptions>>();
                return new BlobStorageServices(options.Value);
            });

            s.TryAddScoped<IContainerService>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<BlobStorageOptions>>();
                return new ContainerService(options.Value);
            });

            return s;
        }

        public static IServiceCollection AddAzureTableStorage(this IServiceCollection s, Action<TableStorageOptions> o)
        {
            s.Configure(o);

            s.AddSingleton(sp => 
            {
                var opts = sp.GetRequiredService<IOptions<TableStorageOptions>>();

                return new TableClient(opts.Value.ConnectionString, opts.Value.TableName);
            });

            s.AddScoped<ITableStorageService, TableStorageService>();

            return s;
        }       
    }
}
