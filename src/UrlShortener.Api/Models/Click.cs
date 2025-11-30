using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Api.Models;

[Index(nameof(ShortCode))]
[Index(nameof(ClickedAt))]
public class Click
{
    public long Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string ShortCode { get; set; } = string.Empty;

    public DateTimeOffset ClickedAt { get; set; } = DateTimeOffset.UtcNow;

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public Url? Url { get; set; }
}
