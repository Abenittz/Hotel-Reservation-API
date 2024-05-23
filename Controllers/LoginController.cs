using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationApi.Models;
using ReservationApi.Services;

namespace ReservationApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly UserServices loginservices;

    public LoginController(UserServices _loginservices)
    {
        loginservices = _loginservices;
    }

    [AllowAnonymous]
[Route("authenticate")]
[HttpPost]
public ActionResult Login([FromBody] LoginRequest loginRequest)
{
    var response = loginservices.Authenticate(loginRequest.Email, loginRequest.Password);

    if (response == null)
    {
        return Unauthorized();
    }

    return Ok(response);
}
    
    [AllowAnonymous]
    [Route("logout")]
    [HttpPost]
    public ActionResult Logout()
    {
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(userEmail))
        {
            return BadRequest("User not authenticated");
        }

        // Use the injected instance of UserServices to revoke or invalidate the user's tokens
        loginservices.RevokeToken(userEmail);

        return Ok(new { Message = "Logout successful" });
    }
}