namespace AzureStorage
{
    public class FileChunk
    {
        public byte[]? Data { get; set; }
        public string? FileName { get; set; }
        public long Offset { get; set; }
        public bool FirstChunk { get; set; }
        public string? ContainerName { get; set; }
    }
}
