using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Api.Models.Dtos;

public class ShortenUrlRequest
{
    [Required]
    [Url]
    public string Url { get; set; } = string.Empty;

    [MaxLength(20)]
    [RegularExpression(@"^[a-zA-Z0-9\-_]+$", ErrorMessage = "Custom alias can only contain alphanumeric characters, hyphens, and underscores.")]
    public string? CustomAlias { get; set; }
}

public class UrlResponse
{
    public string Id { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public string LongUrl { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}
