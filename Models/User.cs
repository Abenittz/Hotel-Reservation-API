using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace ReservationApi.Models;

public class User
{
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("Id")]
    public string? Id { get; set; }

    [BsonElement("FullName")]
    public required string FullName { get; set; }

    [BsonElement("Email")]
    public required string Email { get; set; }

    [BsonElement("Password")]
    public required string Password { get; set; }


    [BsonElement("IsEmailVerified")]
    public bool IsEmailVerified { get; set; }

    [BsonElement("EmailVerificationToken")]
    public string? EmailVerificationToken { get; set; }
 
}
