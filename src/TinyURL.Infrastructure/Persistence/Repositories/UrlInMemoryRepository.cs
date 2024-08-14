using TinyURL.Domain.Urls;

namespace TinyURL.Infrastructure.Persistence.Repositories;

public sealed class UrlInMemoryRepository : IUrlRepository
{

    private readonly IDataStorage<Guid, ShortenedUrl> _dataStorage;

    public UrlInMemoryRepository(IDataStorage<Guid, ShortenedUrl> dataStorage, CancellationToken cancellationToken = default)
    {
        _dataStorage = dataStorage;
    }

    public async Task<bool> CodeExistAsync(string code, CancellationToken cancellationToken = default)
    {
        var found = await _dataStorage.FindByCodeAsync(code, cancellationToken);
        if (found != null)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dataStorage.RemoveAsync(id, cancellationToken);
    }

    public async Task<ShortenedUrl?> GetShortenedUrlByCode(string code, CancellationToken cancellationToken = default)
    {
        return await _dataStorage.FindByCodeAsync(code, cancellationToken);
    }

    public async Task InsertAsync(ShortenedUrl shortenedUrl, CancellationToken cancellationToken = default)
    {
        await _dataStorage.AddAsync(shortenedUrl.Id, shortenedUrl);
    }

    public async Task UpdateAsync(ShortenedUrl shortenedUrl, CancellationToken cancellationToken = default)
    {
        await _dataStorage.UpdateAsync(shortenedUrl.Id, shortenedUrl);
    }
}
