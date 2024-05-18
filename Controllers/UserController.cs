using Microsoft.AspNetCore.Mvc;
using ReservationApi.Models;
using ReservationApi.Services;
using System.Threading.Tasks;

namespace ReservationApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserServices userServices;

    public UserController(UserServices _userService)
    {
         userServices = _userService;
        
    }

    [HttpGet]
    public async Task< ActionResult<List<User>>> GetAllUSers()
    {
        return await userServices.GetAllUsers();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUSer(String id)
    {
        return await userServices.GetUSer(id);
        
    }

    

   [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] User user)
    {
        var exists = await userServices.GetUSerByEmail(user.Email);
        Console.WriteLine(exists);
        if (exists == null){

            var createdUserResponse = await userServices.Create(user);

            return Ok(new
        {
            Message = "User registered successfully. Please check your email for verification.",
            User = createdUserResponse.User,
            Token = createdUserResponse.Token
        });
        }
        return BadRequest("The user already exists");
    }





    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] EmailVerificationRequest request)
    {
        var email = request.Email;
        var token = request.Token;

        var result = await userServices.VerifyEmailAsync(email, token);

        if (result)
        {
            return Ok("Email verification successful");
        }
        else
        {
            return BadRequest("Invalid email or token");
        }
    }




    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var email = request.Email;
        var currentPassword = request.CurrentPassword;
        var newPassword = request.NewPassword;

        var isPasswordChanged = await userServices.ChangePassword(email, currentPassword, newPassword);

        if (isPasswordChanged)
        {
            return Ok(new { Message = "Password changed successfully." });
        }
        else
        {
            return BadRequest("Failed to change password. Please check your current password.");
        }
    }




   


}