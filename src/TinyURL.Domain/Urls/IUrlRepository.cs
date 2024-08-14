namespace TinyURL.Domain.Urls;

public interface IUrlRepository
{
    Task InsertAsync(ShortenedUrl shortenedUrl, CancellationToken cancellationToken = default);

    Task<bool> CodeExistAsync(string code, CancellationToken cancellationToken = default);

    Task<ShortenedUrl> GetShortenedUrlByCode(string code, CancellationToken cancellationToken = default);
    
}
