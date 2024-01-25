

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReservationApi.Models;

public class Hotel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("Id")]
    
    public String? Id { get; set; }

    public required String Name { get; set; }
    public required String Description { get; set; }
}


public class ReservationDBSettings 
{
    public String ConnectionURI { get; set; } = null!;
    public String? DatabaseName { get; set; } = null!;
    public String? HotelCollectionName { get; set; } = null!;
    public String? ReservationCollectionName { get; set; } = null!;
   
}