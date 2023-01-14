using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ZeroX.DB;
using ZeroX.DB.Models;
using ZeroX.DB.Repositories;
using ZeroX.Infrastructure.Extensions;
using ZeroX.Infrastructure.Interfaces;
using ZeroX.Infrastructure.SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret key"));
// Add services to the container.

services.AddControllers();

services.AddScoped<IGameRepository, GameRepository>();

services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddJwt()
    .AddCustomSignalR();

services.AddCors(o =>
    o.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .WithOrigins("http://localhost:8080")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    }));

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateAudience = false,
            ValidateIssuer = false,
        };
    });

// добавление ApplicationDbContext для взаимодействия с базой данных учетных записей
services.AddDbContext<ZeroXContext>(options =>
    //options.UseNpgsql("User Id=postgres;Password=Vselord2002;Host=localhost;Port=5432;Database=zerox")
    options.UseNpgsql($"User Id={config["DB_USERNAME"]};Password={config["DB_PASSWORD"]};Host={config["DB_HOST"]};Database={config["DB_DATABASE"]};Port={config["DB_PORT"]}")
    );

// добавление сервисов Idenity
services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ZeroXContext>()
    .AddUserManager<UserManager<AppUser>>()
    .AddDefaultTokenProviders();

services.AddNewRoomConsumer(config);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/hub");

app.Run();