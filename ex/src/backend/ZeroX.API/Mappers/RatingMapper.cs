using ZeroX.API.ViewModels;
using ZeroX.DB.Models;

namespace ZeroX.API.Mappers;

public static class RatingMapper
{
    public static RatingViewModel IdentityToRating(this AppUser user)
        => new()
        {
            UserName = user.UserName,
            Rate = user.Rate
        };
}