using Microsoft.AspNetCore.Mvc;
using ZeroX.Infrastructure.DTO;
using ZeroX.Infrastructure.Interfaces;
using ZeroX.Infrastructure.Parameters;

namespace ZeroX.API.Controllers;

[Route("game")]
public class GameController : Controller
{
    private readonly IGameRepository _repository;
    
    public GameController(IGameRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    public Task<IEnumerable<GameDto>> Games([FromQuery] GameParameters parameters)
    {
        return Task.FromResult(_repository.GetGames(parameters));
    }
}