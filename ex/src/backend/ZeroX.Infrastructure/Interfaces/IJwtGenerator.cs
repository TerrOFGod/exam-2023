using Microsoft.AspNetCore.Identity;
using ZeroX.Infrastructure.DTO;

namespace ZeroX.Infrastructure.Interfaces;

public interface IJwtGenerator
{
    string CreateToken(UserDto user);
}