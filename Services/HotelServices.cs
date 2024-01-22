using System.ComponentModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ReservationApi.Models;


namespace ReservationApi.Services;

public class HotelServices
{
    private readonly IMongoCollection<Hotel> _hotelCollections;
    private readonly IMongoCollection<RoomReservation> _reservationCollections;
    // private readonly IMongoCollection<Rooms> _roomCollections;
    // private readonly IMongoCollection<RoomsType> _roomsTypeCollections;


    public HotelServices(IOptions<ReservationDBSettings> hotelDBSettings)
    {
        MongoClient client = new MongoClient(hotelDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(hotelDBSettings.Value.DatabaseName);
        _hotelCollections = database.GetCollection<Hotel>(hotelDBSettings.Value.HotelCollectionName);
        _reservationCollections = database.GetCollection<RoomReservation>(hotelDBSettings.Value.ReservationCollectionName);
        

    }

    public async Task<List<Hotel>> GetAllAsync() 
    {
        return await _hotelCollections.Find(r => true).ToListAsync();
    }

    public async Task<Hotel> GetAsync(String id) 
    {
        return await _hotelCollections.Find(r => r.Id == id).FirstOrDefaultAsync() ?? throw new InvalidDataException($"The hotel couldn't not be found");
         
        
    }
    public async Task CreateAsync(Hotel hotel){
        await _hotelCollections.InsertOneAsync(hotel);
        return;
    }
    public async Task UpdateAsync(String id, Hotel hotel)
    {
        var Updated = await _hotelCollections.Find(r => r.Id == id).FirstOrDefaultAsync() ?? throw new InvalidDataException($"The hotel couldn't not be found");
        Updated.Name = hotel.Name;
        Updated.Description = hotel.Description;

        await _hotelCollections.ReplaceOneAsync(r => r.Id == id, Updated);
        return;
    }

    public async Task CancelReservationAsync(String id)
    {

        var removedreservation = await _hotelCollections.Find(r => r.Id == id).FirstOrDefaultAsync();

        if(removedreservation != null){
            await _hotelCollections.DeleteOneAsync(x => x.Id == id);
            return;
        }else{

            throw new InvalidDataException($"the hotel couldnt be found");
        }

        
    }

    
}
