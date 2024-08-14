using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyURL.Infrastructure.Persistence;

public interface IDataStorage<TKey, TValue>
{
    Task<bool> AddAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
    Task<TValue?> GetAsync(TKey key, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsync(TKey key, CancellationToken cancellationToken = default);
    Task<int> CountAsync(TKey key, Func<TValue, int> countSelector, CancellationToken cancellationToken = default);
    Task<TValue?> FindByCodeAsync(string code, CancellationToken cancellationToken = default);
}