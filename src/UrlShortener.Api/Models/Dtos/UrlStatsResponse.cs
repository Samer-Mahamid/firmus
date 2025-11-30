namespace UrlShortener.Api.Models.Dtos;

public class UrlStatsResponse
{
    public long TotalClicks { get; set; }
    public long UniqueVisitors { get; set; }
    public long ClicksLast24H { get; set; }
    public long ClicksLast7D { get; set; }
}
