using Microsoft.AspNetCore.Identity;

namespace ZeroX.DB.Models;

public class AppUser : IdentityUser
{
    public int? Rate { get; set; } = 0;
}