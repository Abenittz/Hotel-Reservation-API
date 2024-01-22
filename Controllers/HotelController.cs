using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ReservationApi.Models;
using ReservationApi.Services;

namespace ReservationApi.HotelController;

[ApiController]
[Route("[controller]")]
public class HotelController : ControllerBase
{
    private readonly HotelServices _hotelServices;

    public HotelController(HotelServices hotelreservations)
    {
        _hotelServices = hotelreservations;
    }

    [HttpGet]
    public async Task<List<Hotel>> GetReservations()
    {
        return await _hotelServices.GetAllAsync();
    }
    [HttpGet("{id}")]
    public async Task<Hotel> GetReservation(String id)
    {
        return await _hotelServices.GetAsync(id);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Hotel hotel)
    {
        await _hotelServices.CreateAsync(hotel);
        return CreatedAtAction(nameof(GetReservations), new {id = hotel.Id}, hotel);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Hotel hotel, String id)
    {
        await _hotelServices.UpdateAsync(id, hotel);     
        return NoContent();
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(String id)
    {
        await _hotelServices.CancelReservationAsync(id);     
        return NoContent();
    }
}
