namespace ReservationApi.DatabaseSettings;
public class ReservationDBSettings 
{
    public String ConnectionURI { get; set; } = null!;
    public String DatabaseName { get; set; } = null!;
    public String HotelCollectionName { get; set; } = null!;
    public String ReservationCollectionName { get; set; } = null!;
    public String UserCollectionName { get; set; } = null!;
    public string RoomCollectionName { get; set; } = null!;
    public string RoomTypeCollectionName { get; set; } = null!;
}