using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ZeroX.API.Extensions;
using ZeroX.DB;
using ZeroX.DB.Models;
using ZeroX.DB.Repositories;
using ZeroX.Infrastructure.Extensions;
using ZeroX.Infrastructure.Interfaces;
using ZeroX.Infrastructure.SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super-secret-key"));
// Add services to the container.

services.AddServices(config, key);

var app = builder.Build();

app.AddMiddlewares();

app.Run();