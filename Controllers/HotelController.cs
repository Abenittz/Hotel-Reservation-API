using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ReservationApi.Models; 
using ReservationApi.Services;

namespace ReservationApi.controllers;

[ApiController]
[Route("[Controller]")]
public class HotelController : ControllerBase{
    private readonly HotelService _hotelService;

    public HotelController(HotelService hotelService){
        _hotelService = hotelService;
    }

    [HttpGet]
    public async Task<List<Hotel>> GetHotelsAsync()
    {
        return await _hotelService.GetAllHotels();
    }

    [HttpGet("{id}")]
    public async Task<Hotel> GetHotelsByIdAsync(String id){
        return await _hotelService.GetHotelById(id);
    }

    [HttpGet("{name}")]
    public async Task<Hotel> GetHotelsByNameAsync(String name){
        return await _hotelService.GetHotelByName(name);
    }

    [HttpPost]
    public async Task<IActionResult> CreateHotel([FromBody] Hotel hotel){
        try{
            await _hotelService.CreateHotel(hotel);

            return CreatedAtAction(nameof(GetHotelsAsync), new { id = hotel.Id}, hotel);
        }catch (InvalidOperationException ex){
            return BadRequest(new { Message = ex.Message});
        }
        catch (Exception) {
            return BadRequest(new { Message = "Failed to create Hotel"});
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteHotel([FromBody] Hotel hotel, String id)
    {
        await _hotelService.DeleteHotelById(id);
        return NoContent();
    }


    
}