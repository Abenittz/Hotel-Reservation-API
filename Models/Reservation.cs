using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReservationApi.Models;

public class RoomReservation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("Id")]
    public string? Id { get; set; }

    [BsonRequired]
    [BsonElement("FullName")]
    public required string FullName { get; set; }

    [BsonRequired]
    [BsonElement("RoomId")]
    public required string RoomId { get; set; } // Foreign key to Room

    [BsonRequired]
    [BsonElement("CheckInDate")]
    public DateTime CheckInDate { get; set; }

    [BsonRequired]
    [BsonElement("CheckOutDate")]
    public DateTime CheckOutDate { get; set; }

    [BsonRequired]
    [BsonElement("PaymentType")]
    public required string PaymentType { get; set; }

}

