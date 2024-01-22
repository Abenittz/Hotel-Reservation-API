using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReservationApi.Models;
public class RoomsType 
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public required string Type { get; set; }
    public decimal Price { get; set; } 
    [BsonRepresentation(BsonType.String)]
    public Guid HotelId { get; set; }  
}