using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ReservationApi.Models; 
using ReservationApi.Services;


namespace ReservationApi.controllers;

[ApiController]
[Route("[controller]")]
public class ReservationController : ControllerBase
{
    private readonly ReservationServices _reservationServices;

    public ReservationController(ReservationServices reservationServices)
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

    [HttpGet("get-by-name/{fullname}")]
    public async Task<RoomReservation> GetByName(String fullname)
    {
        return await _reservationServices.GetByName(fullname);
    }

    [HttpGet("get-available-rooms")]
    public async Task<List<Room>> GetAvailableRooms(){
        return await _reservationServices.GetAvailableAsync();
       
    }

   [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] RoomReservation reservation)
    {
        try
        {
            await _reservationServices.CreateAsync(reservation);
            
            await _reservationServices.SendReservationNotificationAsync(reservation);

            return CreatedAtAction(nameof(GetReservations), new { id = reservation.Id }, reservation);
        }
        catch (InvalidDataException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception)
        {
            return BadRequest(new { Message = "Failed to create reservation" });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateReservation reservation, String id)
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