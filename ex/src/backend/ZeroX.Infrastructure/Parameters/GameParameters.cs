namespace ZeroX.Infrastructure.Parameters;

public class GameParameters
{
    const int maxPageSize = 50;
    public int Page { get; set; } = 1;
    
    private int _pageSize = 10;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}