using ZeroX.DB.Models;
using ZeroX.Infrastructure.DTO;

namespace ZeroX.DB.Mappers;

public static class GameMapper
{
    public static Game DtoToDb(this GameDto game)
        => new()
        {
            Creator = game.Creator,
            CreatorConnection = game.CreatorConnection,
            Opponent = game.Opponent,
            OpponentConnection = game.OpponentConnection,
            CreateDate = game.CreateDate,
            MaxRate = game.MaxRate
        };
    
    public static GameDto DbToDto(this Game? game)
        => new()
        {
            Id = game.Id,
            Creator = game.Creator,
            CreatorConnection = game.CreatorConnection,
            Opponent = game.Opponent,
            OpponentConnection = game.OpponentConnection,
            CreateDate = game.CreateDate,
            MaxRate = game.MaxRate
        };
}