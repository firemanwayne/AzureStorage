using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzureStorage.Abstract
{
    public abstract class AzureTableEntityBase<T> : ITableEntity
    {
        string? partionKey;
        readonly Dictionary<string, object> _properties = new();

        public AzureTableEntityBase()
        {
            RowKey = Guid.NewGuid().ToString();
            IsDeleted = false;
        }

        public AzureTableEntityBase(string rowKey)
        {
            RowKey = rowKey;
        }       

        public string? PartitionKey 
        {
            get => partionKey;
            set => partionKey = typeof(T).Name.ToLowerInvariant();
        }

        [Key]
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }

        [NotMapped]
        public ETag ETag { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public object this[string name]
        {
            get => ContainsProperty(name) ? _properties[name] : null;
            set => _properties[name] = value;
        }

        public ICollection<string> PropertyNames => _properties.Keys;

        public int PropertyCount => _properties.Count;

        public bool ContainsProperty(string name) => _properties.ContainsKey(name);

        public void OnCreated()
        {
            Created = DateTime.Now;
            Modified = DateTime.Now;
        }
        public void OnUpdated()
        {
            Modified = DateTime.Now;
        }
        public void OnDelete()
        {
            IsDeleted = true;
            Modified = DateTime.Now;
        }
    }
}
