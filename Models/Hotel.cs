using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Hotel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("Id")]
    public string? Id { get; set; }

    [BsonRequired]
    [BsonElement("Name")]
    public required string Name { get; set; }

}
