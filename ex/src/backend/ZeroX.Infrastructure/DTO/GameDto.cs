namespace ZeroX.Infrastructure.DTO;

public class GameDto
{
    public int Id { get; set; }
    public string? Creator { get; set; }
    public string? CreatorConnection { get; set; }
    public string? Opponent { get; set; }
    public string? OpponentConnection { get; set; }
    public int? MaxRate { get; set; }
    public DateTime CreateDate { get; set; }
}