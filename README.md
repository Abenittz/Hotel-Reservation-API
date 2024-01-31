# Hotel Reservation Web API (ASP.NET 7.0)

Welcome to the Hotel Reservation Web API built with ASP.NET 7.0 and MongoDB. This API provides a robust set of functionalities for managing hotel reservations, including user authentication, authorization, email verification, change password, reservation creation, cancellation, and more.

## Features

### User Authentication and Authorization

- **Registration:** Users can securely register by providing necessary details, and their passwords are securely hashed before storage.
- **Login:** Registered users can log in to access secured endpoints.
- **Email Verification:** An email verification system ensures the authenticity of user emails. After registration, users receive a verification link via email.
- **Change Password:** Users can securely change their passwords by providing the current and new passwords.

### Reservation Management

- **Create Reservation:** Users can create reservations by providing details such as name, check-in, check-out dates, and room type.
- **Cancel Reservation:** Reserved rooms can be canceled, freeing up availability for other guests.
- **Reserved Rooms:** Reserved Rooms wont be available if they are already reserved.

### Email Notification

- **Email Notification:** Users receive email notifications for important events, such as registration and reservation creation.

## How to Use

1. **Install and Setup .NET:**

   - Ensure that you have [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.
   - Make sure you Download `.NET 7.0`.
   - Open a terminal or command prompt and run the following command to verify the installation:
     ```bash
     dotnet --version
     ```

2. **Clone the Repository:**

   - Begin by cloning the project repository to your local machine.
     ```bash
     git clone https://github.com/Abenittz/Hotel-Reservation-API.git
     ```
   - Locate to the directory
      ```cmd
           cd "the project directory"
      ```
      ```cmd
          dotnet run
      ``` 

3. **Setup MongoDB:**

   - Install and configure MongoDB on your machine. Follow the official [MongoDB installation guide](https://docs.mongodb.com/manual/installation/) for your operating system.
   - Update the `appsettings.json` file with the following MongoDB connection details.
     ```json
     "AllowedHosts": "*",
      "MongoDB": {
        "ConnectionURI": "mongodb://localhost:27017",
        "DatabaseName": "HotelReservation",
        "ReservationCollectionName": "Reservation",
        "UserCollectionName": "Users"
      }
     ```
   - Then add this code to the program.cs file.
      ```cs
        builder.Services.Configure<ReservationDBSettings>(builder.Configuration.GetSection("MongoDB"));
        builder.Services.AddSingleton<ReservationServices>();
        builder.Services.AddScoped<UserServices>();
      ```
   - Then you'r all setup.

4. **Configure Email Settings:**

   To enable email notification functionality, you need to set up an SMTP server. Follow these steps to configure the email settings:

   - **Step 1: Create Ethereal Email Account**
     Visit [ethereal.email](https://ethereal.email/) and create a free account. Ethereal Email provides a fake SMTP server for testing purposes.

   - **Step 2: Generate SMTP Server Credentials**
     After creating your Ethereal Email account, navigate to the dashboard and create a new server. Take note of the generated SMTP server details, including the username and password.

   - **Step 3: Update `appsettings.json`**
     Open the `appsettings.json` file in the project and locate the "EmailSettings" section. Update the following fields with the SMTP server details from Ethereal Email:
     ```json
     "EmailSettings": {
       "SmtpServer": "smtp.ethereal.email",
       "Port": 587, 
       "Username": "your_ethereal_email_username", // paste the username from smtp 
       "Password": "your_ethereal_email_password", // paste the password from smtp
       "SenderEmail": "example@gmail.com",
       "SenderName": "the_hotel_name"
     }
     ```

   - **Step 4: Save Changes**
     Save the changes to the `appsettings.json` file.

   - **Step 5: Build and Run the Application**
     Now that the email settings are configured, build and run the application. The system will use the provided SMTP server details to send email notifications for events such as registration and reservation creation.

5. **Setup JWT Token:**

   - In the `appsettings.json` file, locate the "JwtSettings" section.
   - Update the "Key" field with a secure key for JWT token generation.
   - You can generate a key with this command in the terminal.
     ```cmd
         [Convert]::ToBase64String((Get-Random -Count 32 -InputObject (1..255)))
     ```
   - Then back to the `appsettings.json` file and configure the fields with the apropriate settings
     ```json
         "JwtSettings": {
        "Key": "add_the_generated_key", 
        "Issuer": "http://localhost:your_port",
        "Audience": "https://localhost:your_port;http://localhost:your_port",
        "AccessTokenExpirationMinutes": 60
     }
     ```
   - Add thsi to the program.cs file
     ```cs
      var jwtKey = configuration["JwtSettings:Key"];

        if (string.IsNullOrEmpty(jwtKey))
        {
            // Handle the case where the key is missing or empty
            throw new ApplicationException("JWT Key is missing or empty in configuration.");
        }
        
        Console.WriteLine($"JWT Key from configuration: {jwtKey}");
        
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });
      ```

6. **Build and Run the Application:**

   - Open the solution in your preferred IDE (Visual Studio, VS Code) and build/run the project.

# Endpoints

## Hotel

- **GET /hotel**
  - *Description:* Retrieve a list of all hotels.

- **GET /hotel/get-reservation/{id}**
  - *Description:* Retrieve reservation details for a specific hotel by ID.

- **GET /hotel/get-by-name/{fullname}**
  - *Description:* Search for a hotel by full name.

- **POST /hotel**
  - *Description:* Create a new hotel entry.

- **PUT /hotel**
  - *Description:* Update an existing hotel entry.

- **DELETE /hotel**
  - *Description:* Delete a hotel entry.

## Authentication

- **POST /login/authenticate**
  - *Description:* Authenticate a user. Returns a JWT token upon successful authentication.

- **POST /login/logout**
  - *Description:* Log out the authenticated user.

## User

- **GET /user**
  - *Description:* Retrieve a list of all users.

- **GET /user/{id}**
  - *Description:* Retrieve user details by ID.

- **POST /user/register**
  - *Description:* Register a new user. Requires user details in the request body.

- **POST /user/verify-email**
  - *Description:* Verify the user's email address using the provided verification token.

- **POST /user/change-password**
  - *Description:* Change the user's password. Requires the current and new passwords.

# How to Use Endpoints

## Hotel Endpoints

- Use the `/hotel` endpoints to manage hotel data, including creating, updating, and deleting entries.
- Retrieve reservations and search by hotel name.

## Authentication Endpoints

- Utilize the `/login/authenticate` endpoint to log in and receive a JWT token.
- Log out using the `/login/logout` endpoint.

## User Endpoints

- Register new users with `/user/register`.
- Retrieve user details and change passwords using `/user` endpoints.
- Verify user email with `/user/verify-email`.

# Examples

# Get Hotel List

```http
GET /hotel
```
# Create a New Hotel
```http
POST /hotel
```
Request Body:
```json
    {
      "fullName": "string",
      "roomType": "string",
      "hotelName": "string",
      "roomNumber": 0,
      "checkInDate": "2024-01-31T14:57:08.454Z",
      "checkOutDate": "2024-01-31T14:57:08.454Z",
      "paymentType": "string"
    }
```
# Authenticate User
```http
POST /login/authenticate
```
Request Body:
```json
    {
      "email": "user@example.com",
      "password": "password123"
    }
```
# Register New USer
```http
POST /user/register
```
request Body:
```json
    {
      "fullName": "test demo",
      "email": "test@gmail.com",
      "password": "test123"
    }
```

## Contributions

Feel free to submit issues or pull requests.
