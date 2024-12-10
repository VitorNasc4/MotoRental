using Microsoft.EntityFrameworkCore;
using MotoRental.Infrastructure.Persistence;
using MotoRental.Messaging.Consumer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<MotorcycleConsumer>();

// Para usar o Postgres
var connectionString = builder.Configuration.GetConnectionString("MotoRentalCsPostgres");
builder.Services.AddDbContext<MotoRentalDbContext>
    (option => option.UseNpgsql(connectionString));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();


app.Run();


