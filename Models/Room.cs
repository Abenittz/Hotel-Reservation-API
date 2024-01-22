using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReservationApi.Models;
public class Rooms
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String? Id { get; set; }
        public int RoomNumber { get; set; }   
        [BsonRepresentation(BsonType.String)] 
        public Guid RoomTypeId { get; set; }
        [BsonRepresentation(BsonType.String)] 
        public Guid HotelId { get; set; }  
    }
