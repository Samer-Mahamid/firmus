using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Api.Models;

[Index(nameof(ShortCode), IsUnique = true)]
public class Url
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string ShortCode { get; set; } = string.Empty;

    [Required]
    public string LongUrl { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    public DateTimeOffset? UpdatedAt { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Click> Clicks { get; set; } = new List<Click>();
}
