using UrlShortener.Api.Models;
using UrlShortener.Api.Models.Dtos;

namespace UrlShortener.Api.Services;

public interface IUrlService
{
    Task<Url> ShortenUrlAsync(ShortenUrlRequest request);
    Task<Url?> GetUrlByShortCodeAsync(string shortCode);
    Task DeleteUrlAsync(string shortCode);
    Task RecordClickAsync(string shortCode, string? ipAddress, string? userAgent);
    Task<UrlStatsResponse> GetUrlStatsAsync(string shortCode);
}
