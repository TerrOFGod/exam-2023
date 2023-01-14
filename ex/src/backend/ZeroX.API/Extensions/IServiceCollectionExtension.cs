using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ZeroX.DB;
using ZeroX.DB.Models;
using ZeroX.DB.Repositories;
using ZeroX.Infrastructure.Extensions;
using ZeroX.Infrastructure.Interfaces;

namespace ZeroX.API.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config,
        SymmetricSecurityKey key)
    {
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

        services.AddPublisher(config);
        return services;
    }
}