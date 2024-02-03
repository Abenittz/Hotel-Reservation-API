using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Room
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("Id")]
    public string? Id { get; set; }

    [BsonRequired]
    [BsonElement("RoomNumber")]
    public int RoomNumber { get; set; }

    [BsonRequired]
    [BsonElement("HotelId")]
    public required string HotelId { get; set; } // Foreign key to Hotel

    [BsonRequired]
    [BsonElement("RoomTypeId")]
    public required string RoomTypeId { get; set; } // Foreign key to RooomType

    [BsonElement("IsReserved")]
    public bool IsReserved { get; set; }

}