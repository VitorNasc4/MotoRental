using System;
using System.Text;
using MotoRental.API.Filters;
using MotoRental.Application.Validators;
using MotoRental.Core.Repositories;
using MotoRental.Core.Services;
using MotoRental.Infrastructure.AuthServices;
using MotoRental.Infrastructure.Persistence;
using MotoRental.Infrastructure.Persistence.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MotoRental.Application.Commands.CreateUser;
using MotoRental.Infrastructure.MessageBus;
using MotoRental.Infrastructure.NotificationService;
using System.Linq;
using MotoRental.Core.Entities;

var builder = WebApplication.CreateBuilder(args);

// Para usar o Postgres
var connectionString = builder.Configuration.GetConnectionString("MotoRentalCsPostgres");
builder.Services.AddDbContext<MotoRentalDbContext>
    (option => option.UseNpgsql(connectionString));

// Injeções de dependências
builder.Services.AddMediatR(typeof(CreateUserCommand));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMessageBusService, MessageBusService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Ajustando HttpClient
builder.Services.AddHttpClient();

// Ajustando o ValidationFilters
builder.Services.AddControllers(options => options.Filters.Add(typeof(ValidationFilters)));

// Fluent Validator
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserAdminCommandValidator>();

// Autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MotoRental.API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header usando o esquema Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Aplica as migrations automaticamente ao iniciar a aplicação
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MotoRentalDbContext>();
    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
    try
    {
        dbContext.Database.Migrate();

        var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Role == "admin");
        if (existingUser == null)
        {
            var user = new User("User Admin", "admin@email.com", authService.ComputeSha256Hash("teste123"), RoleTypes.Admin);
        
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MotoRental.API v1"));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();