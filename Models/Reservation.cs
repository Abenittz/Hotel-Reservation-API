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

    [BsonRequired]
    [BsonElement("FullName")]
    public required string FullName { get; set; }

    [BsonRequired]
    [BsonElement("CheckInDate")]
    public DateTime CheckInDate { get; set; }

    [BsonRequired]
    [BsonElement("CheckOutDate")]
    public DateTime CheckOutDate { get; set; }
    [BsonElement("RoomType")]
    public required String RoomType { get; set; }

    [BsonElement("HotelName")]
    public required String HotelName { get; set; }

    [BsonElement("PaymentType")]
    public required String PaymentType { get; set; }
    
    [BsonElement("RooNumber")]
    public int RoomNumber { get; set; }

}

