using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

public class MongoDBService
{

    private readonly IMongoCollection<Playlist> _playlistCollection;

    public MongoDBService()
    {
        var client = new MongoClient("mongodb://admin:admin@localhost:27017");
        var database = client.GetDatabase("presentation");
        _playlistCollection = database.GetCollection<Playlist>("playlist");
    }

    public List<Playlist> GetAll(int amount)
    {
        return _playlistCollection.Find(x => true).Limit(amount).ToList();
    }

    public async Task CreateAsync(Playlist playlist)
    {
        await _playlistCollection.InsertOneAsync(playlist);
    }

    public async Task AddBulk(List<Playlist> playlist)
    {
        var listWrites = new List<WriteModel<Playlist>>();
        listWrites.AddRange(playlist.Select(x => new InsertOneModel<Playlist>(x)));
        await _playlistCollection.BulkWriteAsync(listWrites);
    }
}

public class Playlist
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; } = null!;

    [BsonElement("items")]
    [JsonPropertyName("items")]
    public List<string> Items { get; set; } = null!;

}