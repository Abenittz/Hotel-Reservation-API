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
    [BsonElement("Description")]
    public required string Description { get; set; }
    [BsonElement("Location")]
    public required Location Location { get; set; }
    [BsonRequired]
    [BsonRepresentation(BsonType.String)]
    [BsonElement("Category")]
    public required HotelCategory Category { get; set; }

}

public class Location{
    [BsonElement("Street")]
    public required string Street { get; set; }
    [BsonElement("City")]
    public required string City { get; set; }

}

public enum HotelCategory
{
    TopRated,
    Newest,
    Budget,
    Luxury,
    Boutique
}
