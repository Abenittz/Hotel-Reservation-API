using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Org.BouncyCastle.Tls;
using ReservationApi.DatabaseSettings;
using ReservationApi.Models;


namespace ReservationApi.Services;

public class ReservationServices
{
    private readonly IConfiguration _configuration;
    private readonly IMongoCollection<RoomReservation> _reservationCollections;


    public ReservationServices(IOptions<ReservationDBSettings> hotelDBSettings, IConfiguration configuration)
    {
         _configuration = configuration;
        MongoClient client = new MongoClient(hotelDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(hotelDBSettings.Value.DatabaseName);
        _reservationCollections = database.GetCollection<RoomReservation>(hotelDBSettings.Value.ReservationCollectionName);
        

    }

    public async Task<List<RoomReservation>> GetAllAsync() 
    {
        return await _reservationCollections.Find(r => true).ToListAsync();
    }

    public async Task<RoomReservation> GetAsync(String id) 
    {

        return await _reservationCollections.Find(r => r.Id == id).FirstOrDefaultAsync() ?? throw new InvalidDataException($"The reservation couldn not be found");

         
    }

    public async Task<RoomReservation> GetByName(String fullname) 
    {

        return await _reservationCollections.Find(r => r.FullName == fullname).FirstOrDefaultAsync() ?? throw new InvalidDataException($"The reservation couldn't not be found");

         
    }

    public async Task CreateAsync(RoomReservation reservation){

        var room = await _reservationCollections.Find(r=> r.HotelName == reservation.HotelName && r.RoomNumber == reservation.RoomNumber).FirstOrDefaultAsync();
        if(room == null){
            await _reservationCollections.InsertOneAsync(reservation);
            return;
        }else{
            throw new InvalidDataException($"Room already reserved");
        }
        
    }



    public async Task UpdateAsync(String id, RoomReservation reservation)
    {
        var Updated = await _reservationCollections.Find(r => r.Id == id).FirstOrDefaultAsync() ?? throw new InvalidDataException($"The reservation couldn't not be found");
        Updated.FullName = reservation.FullName;
        Updated.CheckInDate = reservation.CheckInDate;
        Updated.CheckOutDate = reservation.CheckOutDate;

        await _reservationCollections.ReplaceOneAsync(r => r.Id == id, Updated);
        return;
    }

    public async Task CancelReservationAsync(String id)
    {

        var removedreservation = await _reservationCollections.Find(r => r.Id == id).FirstOrDefaultAsync();

        if(removedreservation != null){
            await _reservationCollections.DeleteOneAsync(x => x.Id == id);
            return;
        }else{

            throw new InvalidDataException($"the reservation couldnt be found");
        }

        
    }






    // Token validator method
    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtKey = _configuration["JwtSettings:Key"];

        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new ApplicationException("JWT Key is missing or empty in configuration.");
        }

        var tokenKey = Encoding.ASCII.GetBytes(jwtKey);

        try
        {
            // Decode the JWT token without validation
            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false, // disable signature validation
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            }, out _);

            return true;
        }
        catch (SecurityTokenException)
        {
            return false;
        }
    }


    
}


