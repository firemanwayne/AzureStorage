using Azure.Data.Tables;
using AzureStorage.Abstract;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AzureStorage.Services
{
    public interface ITableStorageService
    {
        void Remove(string partitionKey, string rowKey);

        T Save<T>(T entity) where T : AzureTableEntityBase<T>, new();

        IEnumerable<T> GetAllRows<T>(Expression<Func<T, bool>>? filter) where T : AzureTableEntityBase<T>, new();
    }

    internal class TableStorageService : ITableStorageService
    {
        readonly TableClient _client;
        readonly ILogger<TableStorageService> _logger;
        

        public TableStorageService(
            TableClient client,
            ILogger<TableStorageService> _logger)
        {
            _client = client;
            this._logger = _logger;
        }

        public IEnumerable<T> GetAllRows<T>(Expression<Func<T, bool>>? filter) where T : AzureTableEntityBase<T>, new()
        {
            try
            {
                if (filter != null)
                    return _client
                        .Query(filter)
                        .AsEnumerable();

                else
                    return _client
                         .Query<T>()
                         .AsEnumerable();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);

                return Enumerable.Empty<T>();
            }
        }

        public T Save<T>(T entity) where T : AzureTableEntityBase<T>, new()
        {
            try
            {
                _client.UpsertEntity(entity, TableUpdateMode.Merge);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return entity;
        }

        public void Remove(string partitionKey, string rowKey)
        {
            try
            {
                _client.DeleteEntity(partitionKey, rowKey);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
