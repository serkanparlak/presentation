using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

public class MongoDBService
{

    private readonly IMongoCollection<Playlist> _playlistCollection;

    public MongoDBService()
    {
        var client = new MongoClient("mongodb://admin:admin@mongo:27017/");
        var database = client.GetDatabase("presentation");
        _playlistCollection = database.GetCollection<Playlist>("playlist");
    }

    public async Task<List<Playlist>> GetAllAsync()
    {
        var list = await _playlistCollection.FindAsync(x => true);
        return list.ToList();
    }

    public async Task CreateAsync(Playlist playlist)
    {
        await _playlistCollection.InsertOneAsync(playlist);
    }

    public async Task AddBulk(List<Playlist> playlist)
    {
        await _playlistCollection.BulkWriteAsync((IEnumerable<WriteModel<Playlist>>)playlist);
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