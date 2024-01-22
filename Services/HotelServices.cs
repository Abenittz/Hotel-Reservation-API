using System.ComponentModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ReservationApi.Models;


namespace ReservationApi.Services;

public class ReservationServices
{
    private readonly IMongoCollection<Hotel> _hotelCollections;
    private readonly IMongoCollection<RoomReservation> _reservationCollections;
    // private readonly IMongoCollection<Rooms> _roomCollections;
    // private readonly IMongoCollection<RoomsType> _roomsTypeCollections;


    public ReservationServices(IOptions<ReservationDBSettings> hotelDBSettings)
    {
        MongoClient client = new MongoClient(hotelDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(hotelDBSettings.Value.DatabaseName);
        _hotelCollections = database.GetCollection<Hotel>(hotelDBSettings.Value.HotelCollectionName);
        _reservationCollections = database.GetCollection<RoomReservation>(hotelDBSettings.Value.ReservationCollectionName);
        

    }

    public async Task<List<RoomReservation>> GetAllAsync() 
    {
        return await _reservationCollections.Find(r => true).ToListAsync();
    }

    public async Task<RoomReservation> GetAsync(String id) 
    {
        return await _reservationCollections.Find(r => r.Id == id).FirstOrDefaultAsync() ?? throw new InvalidDataException($"The reservation couldn't not be found");
         
        
    }
    public async Task CreateAsync(RoomReservation reservation){
        await _reservationCollections.InsertOneAsync(reservation);
        return;
    }
    public async Task UpdateAsync(String id, RoomReservation reservation)
    {
        var Updated = await _reservationCollections.Find(r => r.Id == id).FirstOrDefaultAsync() ?? throw new InvalidDataException($"The reservation couldn't not be found");
        Updated.GuestName = reservation.GuestName;
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

    
}

