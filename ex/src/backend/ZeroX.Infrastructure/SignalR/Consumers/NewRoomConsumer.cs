using MassTransit;
using Microsoft.Extensions.Logging;
using ZeroX.Infrastructure.DTO;
using ZeroX.Infrastructure.Interfaces;

namespace ZeroX.Infrastructure.SignalR.Consumers;

public class NewRoomConsumer : IConsumer<GameDto>
{
    private readonly IGameRepository _repository;
    private readonly ILogger<NewRoomConsumer> _logger;

    public NewRoomConsumer(ILogger<NewRoomConsumer> logger, IGameRepository repository)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<GameDto> context)
    {
        try
        {
            _logger.LogInformation("Requested opening of room from: {From}, with max rate: {Max}", 
                context.Message.Creator, context.Message.MaxRate);
            await _repository.AddAsync(context.Message, context.CancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e,
                "Error while opening room. "
                + "Creator: \"{Creator}\". "
                + "Max Rate: \"{Max}\"", 
                context.Message.Creator,
                context.Message.MaxRate);
        }
    }
}