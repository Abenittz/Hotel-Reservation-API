using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ReservationApi.Models;
public class ChangePasswordRequest
{
    [BsonElement("Email")]
    public required string Email { get; set; }
    [BsonElement("CurrentPassword")]
    public required string CurrentPassword { get; set; }
    [BsonElement("NewPassword")]
    public required string NewPassword { get; set; }
}