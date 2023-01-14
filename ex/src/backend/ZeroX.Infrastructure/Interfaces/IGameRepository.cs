using ZeroX.Infrastructure.DTO;
using ZeroX.Infrastructure.Parameters;

namespace ZeroX.Infrastructure.Interfaces;

public interface IGameRepository
{
    IEnumerable<GameDto> GetGames(GameParameters parameters);
    Task AddAsync(GameDto item, CancellationToken token);
    Task<GameDto?> FindByIdAsync(int id);
    Task<GameDto?> FindByCreatorAsync(string creator);
    Task AddOpponent(int id, string opponent, string connection);
    Task RemoveAsync(GameDto item);
}