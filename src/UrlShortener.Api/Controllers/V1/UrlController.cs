using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Models.Dtos;
using UrlShortener.Api.Services;

namespace UrlShortener.Api.Controllers.V1;

[ApiController]
[Route("api/v1")]
public class UrlController : ControllerBase
{
    private readonly IUrlService _urlService;

    public UrlController(IUrlService urlService)
    {
        _urlService = urlService;
    }

    [HttpPost("shorten")]
    public async Task<ActionResult<UrlResponse>> ShortenUrl([FromBody] ShortenUrlRequest request)
    {
        try
        {
            var url = await _urlService.ShortenUrlAsync(request);
            var response = new UrlResponse
            {
                Id = url.ShortCode, // Using ShortCode as ID for response as per example (or we can use DB ID)
                ShortUrl = $"{Request.Scheme}://{Request.Host}/{url.ShortCode}",
                LongUrl = url.LongUrl,
                CreatedAt = url.CreatedAt
            };
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet("urls/{shortCode}")]
    public async Task<ActionResult<UrlResponse>> GetUrlDetails(string shortCode)
    {
        var url = await _urlService.GetUrlByShortCodeAsync(shortCode);
        if (url == null)
        {
            return NotFound();
        }

        // We need click count here, which we haven't implemented yet.
        // For now, return basic details.
        return Ok(new
        {
            id = url.ShortCode,
            short_url = $"{Request.Scheme}://{Request.Host}/{url.ShortCode}",
            long_url = url.LongUrl,
            created_at = url.CreatedAt,
            clicks = 0 // Placeholder
        });
    }

    [HttpDelete("urls/{shortCode}")]
    public async Task<IActionResult> DeleteUrl(string shortCode)
    {
        await _urlService.DeleteUrlAsync(shortCode);
        return NoContent();
    }

    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectUrl(string shortCode)
    {
        var url = await _urlService.GetUrlByShortCodeAsync(shortCode);
        if (url == null)
        {
            return NotFound();
        }

        // Track click (Fire and forget or await depending on requirements)
        // For strict latency, we might want to offload this.
        // But requirements say "Synchronous analytics tracking is fine".
        await _urlService.RecordClickAsync(shortCode, HttpContext.Connection.RemoteIpAddress?.ToString(), Request.Headers.UserAgent);

        return Redirect(url.LongUrl);
    }

    [HttpGet("urls/{shortCode}/stats")]
    public async Task<ActionResult<UrlStatsResponse>> GetUrlStats(string shortCode)
    {
        var url = await _urlService.GetUrlByShortCodeAsync(shortCode);
        if (url == null)
        {
            return NotFound();
        }

        var stats = await _urlService.GetUrlStatsAsync(shortCode);
        return Ok(stats);
    }
}
