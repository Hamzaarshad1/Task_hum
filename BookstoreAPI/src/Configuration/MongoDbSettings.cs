namespace BookstoreAPI.Configuration
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string DatabaseName { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
        public string BooksCollectionName { get; set; } = null!;
    }

    public interface IMongoDbSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
        string BooksCollectionName { get; set; }
    }
}

