using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Principal;
using System.Text;
using MailKit.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MongoDB.Driver;
using Org.BouncyCastle.Tls;
using ReservationApi.DatabaseSettings;
using ReservationApi.Models;

using MailKit.Net.Smtp;
using System.Net;



namespace ReservationApi.Services;

public class ReservationServices
{
    
    private readonly IConfiguration _configuration;
    private readonly IMongoCollection<RoomReservation> _reservationCollections;
    private readonly IMongoCollection<Hotel> _hotelCollections;
    private readonly IMongoCollection<Room> _roomCollections;
    private readonly IMongoCollection<RoomType> _roomTypeCollections;


    public ReservationServices(IOptions<ReservationDBSettings> hotelDBSettings, IConfiguration configuration)
    {
        _configuration = configuration;
        MongoClient client = new MongoClient(hotelDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(hotelDBSettings.Value.DatabaseName);
        _reservationCollections = database.GetCollection<RoomReservation>(hotelDBSettings.Value.ReservationCollectionName);
        _hotelCollections = database.GetCollection<Hotel>(hotelDBSettings.Value.HotelCollectionName);
        _roomCollections = database.GetCollection<Room>(hotelDBSettings.Value.RoomCollectionName);
        _roomTypeCollections = database.GetCollection<RoomType>(hotelDBSettings.Value.RoomTypeCollectionName);
        

    }

    public async Task<List<RoomReservation>> GetAllAsync() 
    {
        return await _reservationCollections.Find(r => true).ToListAsync();
    }

    public async Task<RoomReservation> GetAsync(String id) 
    {

        return await _reservationCollections.Find(r => r.Id == id).FirstOrDefaultAsync() ?? throw new InvalidDataException($"The reservation couldn't not be found");

        
    }

    public async Task<RoomReservation> GetByName(String fullname) 
    {

        return await _reservationCollections.Find(r => r.FullName == fullname).FirstOrDefaultAsync() ?? throw new InvalidDataException($"The reservation couldn't not be found");

        
    }

    public async Task<List<Room>> GetAvailableAsync()
    {
        var available = await _roomCollections.Find(r => !r.IsReserved).ToListAsync();

        if(available.Count == 0){
            return null;
        }

        return available;
    }

    public async Task CreateAsync(RoomReservation reservation){

        var room = await _roomCollections.Find(r=> r.Id == reservation.RoomId && !r.IsReserved).FirstOrDefaultAsync();
        if(room != null){
            room.IsReserved = true;
            await _roomCollections.ReplaceOneAsync(r => r.Id == reservation.RoomId, room);

            await _reservationCollections.InsertOneAsync(reservation);
            return;
        }else{
            throw new InvalidDataException($"Room already reserved");
        }
        
    }

    public async Task SendReservationNotificationAsync(RoomReservation reservation)
    {
        try
        {
            var emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();

            var subject = "New Reservation Created";
            var body = $@"
                <p>A new reservation has been created with the following details:</p>
                <ul>
                    <li>Full Name: {reservation.FullName}</li>
                    <li>Check-in Date: {reservation.CheckInDate}</li>
                    <li>Check-out Date: {reservation.CheckOutDate}</li>
                    <li>Payment Type: {reservation.PaymentType}</li>
                </ul>
            ";

            // Specify the full namespace for SmtpClient to avoid ambiguity
            using (var client = new System.Net.Mail.SmtpClient(emailSettings.SmtpServer, emailSettings.SmtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(emailSettings.SmtpUsername, emailSettings.SmtpPassword);
                client.EnableSsl = true;

                var message = new MailMessage
                {
                    From = new MailAddress(emailSettings.SenderEmail, emailSettings.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                // Replace with the actual recipient's email address
                message.To.Add(new MailAddress("recipient@example.com"));

                await client.SendMailAsync(message);
            }
        }
        catch (Exception ex)
        {
            // Handle email sending exceptions
            Console.WriteLine($"Error sending email notification: {ex.Message}");
        }
    }




    public async Task UpdateAsync(string id, UpdateReservation reservation)
    {
        var existingReservation = await _reservationCollections.Find(r => r.Id == id).FirstOrDefaultAsync();

        if (existingReservation == null)
        {
            throw new InvalidDataException($"The reservation with Id {id} could not be found.");
        }

        if (reservation.FullName != null)
        {
            existingReservation.FullName = reservation.FullName;
        }

        if (reservation.CheckInDate.HasValue)
        {
            existingReservation.CheckInDate = reservation.CheckInDate.Value;
        }

        if (reservation.CheckOutDate.HasValue)
        {
            existingReservation.CheckOutDate = reservation.CheckOutDate.Value;
        }

        if (!string.IsNullOrEmpty(reservation.PaymentType))
        {
            existingReservation.PaymentType = reservation.PaymentType;
        }

        await _reservationCollections.ReplaceOneAsync(r => r.Id == id, existingReservation);
    }


    public async Task CancelReservationAsync(String id)
    {

        var removedreservation = await _reservationCollections.Find(r => r.Id == id).FirstOrDefaultAsync();

        if(removedreservation != null){
            await _reservationCollections.DeleteOneAsync(r => r.Id == id);


            var room = await _roomCollections.Find(r => r.Id == removedreservation.RoomId).FirstOrDefaultAsync();
            if(room != null){
                room.IsReserved = false;
                await _roomCollections.ReplaceOneAsync(r => r.Id == removedreservation.RoomId, room);
                
            }
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


