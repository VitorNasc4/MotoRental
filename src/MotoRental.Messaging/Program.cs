using Microsoft.EntityFrameworkCore;
using MotoRental.Infrastructure.Persistence;
using MotoRental.Messaging.Consumer;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders(); 
builder.Logging.AddConsole(); 
builder.Logging.AddDebug();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<MotorcycleConsumer>();

var DB_HOST = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";

// Para usar o Postgres
var rawConnectionString = builder.Configuration.GetConnectionString("MotoRentalCsPostgres");
var connectionString = rawConnectionString!.Replace("DB_HOST", DB_HOST);
builder.Services.AddDbContext<MotoRentalDbContext>
    (option => option.UseNpgsql(connectionString));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();


app.Run();


