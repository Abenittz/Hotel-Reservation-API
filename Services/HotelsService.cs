
using Microsoft.Extensions.Options;
using MongoDB.Driver;

using ReservationApi.DatabaseSettings;


namespace ReservationApi.Services;

public class HotelService{
    private readonly IConfiguration _configuration;
    private readonly IMongoCollection<Hotel> _hotelCollections;
   


    public HotelService(IOptions<ReservationDBSettings> hotelDBSetting, IConfiguration configuration){
        _configuration = configuration;
        MongoClient client = new MongoClient(hotelDBSetting.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(hotelDBSetting.Value.DatabaseName);
        _hotelCollections = database.GetCollection<Hotel>(hotelDBSetting.Value.HotelCollectionName);
        
    }

    public async Task<List<Hotel>> GetAllHotels(){
        return await _hotelCollections.Find(r => true).ToListAsync();
    }

    public async Task<Hotel> GetHotelById(String id){

        return await _hotelCollections.Find(r => r.Id == id).FirstOrDefaultAsync() ?? throw new InvalidDataException("Hotel not exist in database");
        
    }

    public async Task<Hotel> GetHotelByName(String name){
        return await _hotelCollections.Find(r => r.Name == name).FirstOrDefaultAsync() ?? throw new Exception("hotel not found");
    }
public async Task<Hotel> GetHotelByCategory(HotelCategory category)
{
    var categoryString = category.ToString(); // Convert enum to string
    return await _hotelCollections.Find(r => r.Category == categoryString).FirstOrDefaultAsync() 
           ?? throw new Exception("There is no hotel with this category");
}

    public async Task<Hotel> GetHotelByCategoryString(string categoryString)
{
    if (!Enum.TryParse<HotelCategory>(categoryString, true, out var category))
    {
        throw new ArgumentException("Invalid category", nameof(categoryString));
    }

    return await GetHotelByCategory(category);
}

    public async Task CreateHotel(Hotel hotel){
        var hotelName = _hotelCollections.Find(r => r.Name == hotel.Name).FirstOrDefaultAsync();

        if(hotelName == null){
            await _hotelCollections.InsertOneAsync(hotel);
        }else{
            throw new Exception("Hotel already exists");
        }
    }

    public async Task DeleteHotelById(String id){
        var hotelId = _hotelCollections.Find(r => r.Id == id).FirstOrDefaultAsync();
        
        if(hotelId == null){
            await _hotelCollections.DeleteOneAsync(r => r.Id == id);
        }else{
            throw new Exception("Hotel doesn't exist");
        }
    }

}