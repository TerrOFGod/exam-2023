using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ZeroX.Infrastructure.DTO;
using ZeroX.Infrastructure.Interfaces;
using ZeroX.Infrastructure.Parameters;
using ZeroX.Infrastructure.SignalR.Hubs;

namespace ZeroX.API.Controllers;

[Route("game")]
public class GameController : Controller
{
    private readonly ILogger<GameController> _logger;
    private readonly IHubContext<ChatHub> _context;
    private readonly HubCallerContext Context;
    private readonly IGameRepository _repository;
    
    public GameController(ILogger<GameController> logger, IHubContext<ChatHub> context, 
        IGameRepository repository, HubCallerContext context1)
    {
        _logger = logger;
        _context = context;
        _repository = repository;
        Context = context1;
    }
    
    [HttpGet]
    public Task<IEnumerable<GameDto>> Games([FromQuery] GameParameters parameters)
    {
        return Task.FromResult(_repository.GetGames(parameters));
    }
    
    [HttpPost]
    [Route("connect")]
    public async Task<IActionResult> Connect(int id)
    {
        await _repository.AddOpponent(id, Context.User?.Identity?.Name!, Context.ConnectionId);
        return Ok(Context.User?.Identity?.Name!);
    }
}