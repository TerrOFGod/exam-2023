using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using ZeroX.DB.Models;

namespace ZeroX.DB;

public class ZeroXContext : IdentityDbContext<AppUser>
{
    public DbSet<Game> Games { get; set; }
    
    public ZeroXContext(DbContextOptions<ZeroXContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}