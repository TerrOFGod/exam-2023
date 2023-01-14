using ZeroX.DB.Models;
using ZeroX.Infrastructure.DTO;

namespace ZeroX.DB.Mappers;

public static class UserMapper
{
    public static UserDto IdentityToDto(this AppUser user)
        => new()
        {
            UserName = user.UserName
        };
}