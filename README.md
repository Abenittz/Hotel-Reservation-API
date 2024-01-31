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

1. **Clone the Repository:** Begin by cloning the project repository to your local machine.

    ```bash
    git clone https://github.com/Abenittz/hotel-reservation-api.git
    ```

2. **Setup MongoDB:** Ensure that you have MongoDB installed and configured. Update the `appsettings.json` file with your MongoDB connection details.

3. **Configure Email Settings:**

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
       "SenderName": "the hotel name"
     }
     ```

   - **Step 4: Save Changes**
     Save the changes to the `appsettings.json` file.

   - **Step 5: Build and Run the Application**
     Now that the email settings are configured, build and run the application. The system will use the provided SMTP server details to send email notifications for events such as registration and reservation creation.


4. **Build and Run the Application:** Open the solution in your preferred IDE (Visual Studio, VS Code) and build/run the project.

## Contributions

Feel free to submit issues or pull requests.
