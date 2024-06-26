using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using ReservationApi.DatabaseSettings;
using ReservationApi.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using System.Web;
using Microsoft.OpenApi.Extensions;


namespace ReservationApi.Services;

public class UserServices
{
    private readonly IMongoCollection<User> _userCollections;
    private readonly IMongoCollection<ChangePasswordRequest> _resetPasswordCollections;
    private readonly IConfiguration _configuration;
    private readonly List<string> _tokenBlacklist = new List<string>();


   public UserServices(IOptions<ReservationDBSettings> hotelDBSettings, IConfiguration configuration)
        {
            _configuration = configuration; // Add this line
            MongoClient client = new MongoClient(hotelDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(hotelDBSettings.Value.DatabaseName);
            _userCollections = database.GetCollection<User>(hotelDBSettings.Value.UserCollectionName);
            _resetPasswordCollections = database.GetCollection<ChangePasswordRequest>(hotelDBSettings.Value.UserCollectionName);
        }


    public async Task<List<User>> GetAllUsers() => await _userCollections.Find(r => true).ToListAsync();
    public async Task<User> GetUSer(String id) => await _userCollections.Find<User>(r => r.Id == id).FirstOrDefaultAsync();
    public async Task<User> GetUSerByEmail(String email) => await _userCollections.Find<User>(r => r.Email == email).FirstOrDefaultAsync();

  
    public async Task<UserResponse> Create(User user)
    {
        user.IsEmailVerified = false;
        var token = GenerateToken(user.Email, user.FullName);

        // Hash the password before storing it
        user.Password = HashPassword(user.Password);

        await _userCollections.InsertOneAsync(user);

        // Send email with verification link
        // await SendEmailVerificationEmail(user.Email, user.EmailVerificationToken);
        await SendEmailVerificationEmail(user.Email, token);

       var response = new UserResponse(user, token);
       return response;
    }

   // hashing the password before decrypting it 
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }


public class LoginResponse
{
    public string Token { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public bool IsEmailVerified { get; set; }
    // Add other user properties as needed

    public LoginResponse(User user, string token)
    {
        Token = token;
        Email = user.Email;
        FullName = user.FullName;
        IsEmailVerified = user.IsEmailVerified;
      
    }
}



public LoginResponse Authenticate(string email, string password)
{
    // Find the user by email
    var user = _userCollections.Find(x => x.Email == email).FirstOrDefault();

    if (user == null || !VerifyPassword(password, user.Password))
    {
        return null;
    }

    var token = GenerateToken(user.Email, user.FullName);

    // Create the response object with user data and token
    var response = new LoginResponse(user, token);

    return response;
}

    public void RevokeToken(string email)
    {
        // Add the user's token to the blacklist
        var userToken = _tokenBlacklist.FirstOrDefault(t => t.Contains(email));
        if (userToken == null)
        {
            _tokenBlacklist.Add(userToken);
        }
    }

    public bool IsTokenBlacklisted(string token)
    {
        // Check if the token is in the blacklist
        return _tokenBlacklist.Contains(token);
    }

    // verify the hashed password using BCrypt
    private bool VerifyPassword(string enteredPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
    }


    // token generator based on email
    public string GenerateToken(string email, string fullname)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtKey = _configuration["JwtSettings:Key"];

        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new ApplicationException("JWT Key is missing or empty in configuration.");
        }

        var tokenKey = Encoding.ASCII.GetBytes(jwtKey);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]{
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Name, fullname)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
  
        var token = tokenHandler.CreateToken(tokenDescriptor);         
        var tokenString = tokenHandler.WriteToken(token);         
     
        return tokenString;
    }






    // email verification logic
    public async Task SendEmailVerificationEmail(string userEmail, string verificationToken)
    {
        var emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();

        var subject = "Email Verification";
        var body = $"Click the following link to verify your email: {GetVerificationLink(userEmail, verificationToken)}";

        using (var client = new SmtpClient(emailSettings.SmtpServer, emailSettings.SmtpPort))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(emailSettings.SmtpUsername, emailSettings.SmtpPassword);
            client.EnableSsl = true;

            var message = new MailMessage
            {
                From = new MailAddress(emailSettings.SenderEmail, emailSettings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(userEmail));

            await client.SendMailAsync(message);
        }
    }



    private string GetVerificationLink(string userEmail, string verificationToken)
    {
        
        var verificationEndpoint = "https://yourwebsite.com/verify-email";

        return $"{verificationEndpoint}?email={HttpUtility.UrlEncode(userEmail)}&token={HttpUtility.UrlEncode(verificationToken)}";
        
    }


    public async Task<bool> VerifyEmailAsync(string email, string token)
    {
        var user = await _userCollections.Find(x => x.Email == email).FirstOrDefaultAsync();

        if (user != null)
        {
            // Mark email as verified
            user.IsEmailVerified = true;
            
            // Save the updated user document
            await _userCollections.ReplaceOneAsync(x => x.Id == user.Id, user);

            return true;
        }

        return false;
    }






    public async Task<bool> ChangePassword(string email, string currentPassword, string newPassword)
    {
        var user = await _userCollections.Find(x => x.Email == email && x.IsEmailVerified).FirstOrDefaultAsync();

        if (user == null)
        {
            return false;
        }

        if (!VerifyPassword(currentPassword, user.Password))
        {
            return false;
        }

        user.Password = HashPassword(newPassword);

        await _userCollections.ReplaceOneAsync(x => x.Id == user.Id, user);

        return true;
    }

}


public class UserResponse
{
    public User User { get; set; }
    public string Token { get; set; }

    public UserResponse(User user, string token)
    {
        User = user;
        Token = token;
    }
}