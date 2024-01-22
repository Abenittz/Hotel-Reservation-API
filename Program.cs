using ReservationApi.Models;
using ReservationApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ReservationDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<ReservationServices>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
