using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ReservationApi.Models; 
using ReservationApi.Services;


namespace ReservationApi.controllers;

[ApiController]
[Route("[controller]")]
public class HotelController : ControllerBase
{
    private readonly ReservationServices _reservationServices;

    public HotelController(ReservationServices reservationServices)
    {
        _reservationServices = reservationServices;
    }

    [HttpGet]
    public async Task<List<RoomReservation>> GetReservations()
    {
        return await _reservationServices.GetAllAsync();
    }
    
    [HttpGet("get-reservation/{id}")]
    public async Task<RoomReservation> GetReservation(String id)
    {
        return await _reservationServices.GetAsync(id);
    }

    [HttpGet("get-by-name/{firstname}")]
    public async Task<RoomReservation> GetByName(String firstname)
    {
        return await _reservationServices.GetByName(firstname);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RoomReservation reservation)
    {

       await _reservationServices.CreateAsync(reservation);
        return CreatedAtAction(nameof(GetReservations), new {id = reservation.Id}, reservation);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] RoomReservation reservation, String id)
    {
         await _reservationServices.UpdateAsync(id, reservation);     
        return NoContent();
    }


    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] RoomReservation reservation, String id)
    {
      
        await _reservationServices.CancelReservationAsync(id);     
        return NoContent();
    }
}