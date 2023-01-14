using MassTransit;
using Microsoft.AspNetCore.SignalR;
using ZeroX.Infrastructure.DTO;
using ZeroX.Infrastructure.Interfaces;

namespace ZeroX.Infrastructure.SignalR.Hubs;

public class ChatHub : Hub
{
    private IGameRepository _gameRepository;
    private readonly IBus _bus;

    public ChatHub(IGameRepository gameRepository, IBus bus)
    {
        _gameRepository = gameRepository;
        _bus = bus;
    }

    public async Task OpenRoom(int maxRate)
    {
        var game = new GameDto
        {
            Creator = Context.User.Identity?.Name,
            CreatorConnection = Context.ConnectionId,
            Opponent = null,
            OpponentConnection = null,
            CreateDate = DateTime.Now,
            MaxRate = maxRate
        };
        var publish = _bus.Publish(game);
        var send = Clients.All.SendAsync("NewRoom", game);
        await Groups.AddToGroupAsync(game.CreatorConnection, game.Creator);
        await Task.WhenAll(publish, send);
    }
    
    public async Task ConnectToRoom(int gameId)
    {
        var game = await _gameRepository.FindByIdAsync(gameId);
        
        var msg = "Welcome to " + game!.Creator + " room";
        await Groups.AddToGroupAsync(Context.ConnectionId, game.Creator);
        await Clients.Caller.SendAsync("GroupChatHub", msg);
        await Clients.OthersInGroup(game.Creator).SendAsync("NewConnection", game.Creator + " " 
            + Context.User.Identity?.Name!);
    }
    
    public async Task ConnectAsOpponent(int id)
    {
        await _gameRepository.AddOpponent(id, Context.User.Identity?.Name!, Context.ConnectionId);
    }
    
    public async Task Turn(int gameId)
    {
        var game = await _gameRepository.FindByIdAsync(gameId);
        
        if (Context.ConnectionId == game.CreatorConnection || Context.ConnectionId == game.OpponentConnection)
            await Clients.Group(game.Creator).SendAsync("PlayerTurnEvent");
        else
            await Clients.Group(game.Creator).SendAsync("NotPlayerTurnEvent");
    }
    
    public async Task SendMassage(MassageDto dto)
    {
        var game = await _gameRepository.FindByIdAsync(dto.GameId);
        var massage = Context.User.Identity?.Name! + ": " + dto.Massage;
        
        await Clients.Group(game!.Creator).SendAsync("MassagePublished", massage);
    }
}