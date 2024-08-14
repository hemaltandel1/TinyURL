namespace TinyURL.Presentation;
public interface IUrlShortener
{
    Task<string> GenerateShortUrlAsync(string longUrl, string customUrl = null, CancellationToken cancellation = default);
    Task<string> GetLongUrlAsync(string shortUrl, CancellationToken cancellation = default);
    Task<bool> DeleteShortUrlAsync(string shortUrl, CancellationToken cancellation = default);
    Task<int> GetStatisticsAsync(string shortUrl, CancellationToken cancellation = default);
}
