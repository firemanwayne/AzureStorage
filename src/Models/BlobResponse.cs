using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureStorage
{
    public class BlobResponse
    {
        public BlobResponse() 
        { }

        public BlobResponse(BlobClient client)
        {
            Name = client.Name;
            Properties = client.GetProperties();
            ContainerName = client.BlobContainerName;
            BlobUri = client.Uri;           

            IsSuccessful = true;
        }

        public string? Name { get; set; }       
        public string? ContainerName { get; set; }
        public BlobProperties? Properties { get; set; }
        public Uri? BlobUri { get; set; }
        public bool IsSuccessful { get; set; }
    }

    public class BlobPropertiesResponse
    {
        public DateTimeOffset? LastModified { get; set; }
        public string? DestinationSnapshot { get; set; }
        public int? RemainingRetentionDays { get; set; }              
        public bool AccessTierInferred { get; set; }              
        public string? CustomerProvidedKeySha256 { get; set; }       
        public string? EncryptionScope { get; set; }      
        public long? TagCount { get; set; }        
        public DateTimeOffset? ExpiresOn { get; set; }        
        public bool? IsSealed { get; set; }               
        public DateTimeOffset? LastAccessedOn { get; set; }       
        public ETag? ETag { get; set; }       
        public DateTimeOffset? CreatedOn { get; set; }        
        public DateTimeOffset? CopyCompletedOn { get; set; }        
        public bool? IncrementalCopy { get; set; }        
        public DateTimeOffset? DeletedOn { get; set; }       
        public bool? ServerEncrypted { get; set; }       
        public string? CopyProgress { get; set; }      
        public long? ContentLength { get; set; }      
        public string? ContentType { get; set; }      
        public string? ContentEncoding { get; set; }     
        public string? ContentLanguage { get; set; }        
        public byte[]? ContentHash { get; set; }      
        public string? ContentDisposition { get; set; }      
        public string? CacheControl { get; set; }      
        public long? BlobSequenceNumber { get; set; }        
        public string? CopyId { get; set; }            
        public Uri? CopySource { get; set; }       
        public string? CopyStatusDescription { get; set; }        
        public DateTimeOffset? AccessTierChangedOn { get; set; }
    }
}
