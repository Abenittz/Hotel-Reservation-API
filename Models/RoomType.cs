using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class RoomType
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("Id")]
    public string? Id { get; set; }

    [BsonRequired]
    [BsonElement("Type")]
    public required string Type { get; set; }

}
