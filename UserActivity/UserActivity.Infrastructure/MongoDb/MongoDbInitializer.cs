using MongoDB.Driver;

namespace UserActivity.Infrastructure.MongoDb;

public class MongoDbInitializer
{
    private readonly IMongoDatabase _database;

    public MongoDbInitializer()
    {
        var connectionString = "mongodb://admin:123456@localhost:27017/ActivityDb?authSource=admin";
        
        var client = new MongoClient(connectionString);

        _database = client.GetDatabase("ActivityDb");
    }

    public void CreateCollectionIfNotExists(string collectionName)
    {
        var collectionNames = _database.ListCollectionNames().ToList();
        if (collectionNames.Contains(collectionName)) return;
        
        _database.CreateCollection(collectionName);
        Console.WriteLine($"Collection '{collectionName}' Created..");
    }
}