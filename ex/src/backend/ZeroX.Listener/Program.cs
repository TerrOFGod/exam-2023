using Microsoft.EntityFrameworkCore;
using ZeroX.DB;
using ZeroX.DB.Repositories;
using ZeroX.Infrastructure.Extensions;
using ZeroX.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

// Add services to the container.
services.AddScoped<IGameRepository, GameRepository>();
services.AddCustomSignalR();

// добавление ApplicationDbContext для взаимодействия с базой данных учетных записей
services.AddDbContext<ZeroXContext>(options =>
    //options.UseNpgsql("User Id=postgres;Password=Vselord2002;Host=localhost;Port=5432;Database=zerox")
    options.UseNpgsql($"User Id={config["DB_USERNAME"]};Password={config["DB_PASSWORD"]};Host={config["DB_HOST"]};Database={config["DB_DATABASE"]};Port={config["DB_PORT"]}")
);

services.AddNewRoomConsumer(config);

var app = builder.Build();

app.Run();