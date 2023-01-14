using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZeroX.DB.Mappers;
using ZeroX.DB.Models;
using ZeroX.Infrastructure.DTO;
using ZeroX.Infrastructure.Interfaces;
using ZeroX.Infrastructure.Parameters;

namespace ZeroX.DB.Repositories;

public class GameRepository : IGameRepository
{
    private readonly ZeroXContext _context;
    private readonly ILogger<GameRepository> _logger;

    public GameRepository(ZeroXContext context, ILogger<GameRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    // ReSharper disable once MethodOverloadWithOptionalParameter
    public IEnumerable<GameDto> GetGames(GameParameters parameters)
    {
        return _context.Games
            .OrderByDescending(game => game.Opponent == null)
            .ThenBy(game => game.CreateDate)
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .Select(game => game.DbToDto())
            .ToList();
    }

    public async Task AddAsync(GameDto item, CancellationToken token = default)
    {
        var dbMessage = item.DtoToDb();
        await _context.AddAsync(dbMessage, token);
        await _context.SaveChangesAsync(token);
        _logger.LogInformation("Sucsessfully add new game to db.");
    }

    public async Task<GameDto?> FindByIdAsync(int id)
    {
        return (await _context.Games.Where(game => game.Id == id)
            .SingleOrDefaultAsync())
            .DbToDto();
    }

    public async Task AddOpponent(int id, string opponent, string connection)
    {
        var game = (await FindByIdAsync(id))!.DtoToDb();
        game.Id = id;
        game.Opponent = opponent;
        game.OpponentConnection = connection;
        _context.Games.Update(game);
        await _context.SaveChangesAsync();
    }

    public Task RemoveAsync(GameDto item)
    {
        throw new NotImplementedException();
    }
}