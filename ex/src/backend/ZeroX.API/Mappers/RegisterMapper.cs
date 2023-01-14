using ZeroX.API.ViewModels;

namespace ZeroX.API.Mappers;

public static class RegisterMapper
{
    public static LoginViewModel RegisterToLogin(this RegisterViewModel register)
        => new()
        {
            Email = register.Email,
            Password = register.Password,
            RememberMe = false
        };
}