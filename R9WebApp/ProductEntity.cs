using Azure;
using Azure.Data.Tables;
using System;

namespace R9WebApp
{
    public class ProductEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public int productId { get; set; }
        public string productName { get; set; }
    }
}
