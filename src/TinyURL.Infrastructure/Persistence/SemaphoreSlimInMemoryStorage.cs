using TinyURL.Domain.Urls;

namespace TinyURL.Infrastructure.Persistence;

public class SemaphoreSlimInMemoryDataStorage<TKey, TValue> : IDataStorage<TKey, TValue>
    where TKey : notnull
    where TValue : class
{
    private readonly Dictionary<TKey, TValue> _store = new Dictionary<TKey, TValue>();
    private readonly Dictionary<string, TKey> _codeIndex = new Dictionary<string, TKey>(); // Index for fast retrieval by Code
    private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

    public async Task<bool> AddAsync(TKey key, TValue value, CancellationToken cancellationToken = default)
    {
        await _semaphoreSlim.WaitAsync(cancellationToken);
        try
        {
            if (_store.ContainsKey(key))
            {
                return false;
            }
            _store[key] = value;

            if (value is ShortenedUrl shortenedUrl)
            {
                _codeIndex[shortenedUrl.Code] = key; // Add to the code index
            }

            return true;
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task<TValue?> GetAsync(TKey key, CancellationToken cancellationToken = default)
    {
        await _semaphoreSlim.WaitAsync(cancellationToken);
        try
        {
            _store.TryGetValue(key, out var value);
            return value;
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task<bool> UpdateAsync(TKey key, TValue value, CancellationToken cancellationToken = default)
    {
        await _semaphoreSlim.WaitAsync(cancellationToken);
        try
        {
            if (!_store.ContainsKey(key))
            {
                return false;
            }
            _store[key] = value;

            if (value is ShortenedUrl shortenedUrl)
            {
                _codeIndex[shortenedUrl.Code] = key; // Update the code index
            }

            return true;
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task<bool> RemoveAsync(TKey key, CancellationToken cancellationToken = default)
    {
        await _semaphoreSlim.WaitAsync(cancellationToken);
        try
        {
            if (_store.Remove(key, out var value))
            {
                if (value is ShortenedUrl shortenedUrl)
                {
                    _codeIndex.Remove(shortenedUrl.Code); // Remove from the code index
                }

                return true;
            }
            return false;
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task<int> CountAsync(TKey key, Func<TValue, int> countSelector, CancellationToken cancellationToken = default)
    {
        await _semaphoreSlim.WaitAsync(cancellationToken);
        try
        {
            if (_store.TryGetValue(key, out var value))
            {
                return countSelector(value);
            }
            return -1;
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task<TValue?> FindByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        await _semaphoreSlim.WaitAsync(cancellationToken);
        try
        {
            if (_codeIndex.TryGetValue(code, out var key) && _store.TryGetValue(key, out var value))
            {
                return value;
            }
            return null;
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
}