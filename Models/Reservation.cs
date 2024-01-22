using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReservationApi.Models;

public class RoomReservation 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("Id")]
    
    public String? Id { get; set; }

    // [BsonRepresentation(BsonType.String)] // Use BsonType.String representation for Guid
    // public Guid RoomId { get; set; }

    // [BsonRepresentation(BsonType.String)] // Use BsonType.String representation for Guid
    // public Guid HotelId { get; set; }

    // [BsonRequired]
    // public required string HotelName { get; set; }

    // public int RoomNumber { get; set; }

    // [BsonRepresentation(BsonType.String)] // Use BsonType.String representation for Guid
    // public Guid UserId { get; set; }

    // [BsonRequired]
    // public required string Type { get; set; }

    // [BsonRepresentation(BsonType.String)] // Use BsonType.String representation for Guid
    // public Guid RoomTypeId { get; set; }

    [BsonRequired]
    public required string GuestName { get; set; }

    [BsonRequired]
    public DateTime CheckInDate { get; set; }

    [BsonRequired]
    public DateTime CheckOutDate { get; set; }
}

