using System.Text.Json;
using ForumNG.Domain.Entities;
using ForumNG.Domain.Interfaces;
using ForumNG.Infrastructure.Data;
using Microsoft.Extensions.Caching.Distributed;

namespace ForumNG.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context, IDistributedCache cache) : IUserRepository
{
    private const string CacheKeyPrefix = "users:";

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        User? user = await GetFromCacheAsync(id, ct);
        if (user is not null)
        {
            return user;
        }

        user = await GetFromDbAsync(id, ct);
        if (user is not null)
        {
            await SaveToCacheAsync(user, ct);
        }
        return user;
    }

    private async Task<User?> GetFromCacheAsync(Guid id, CancellationToken ct = default)
    {
        string cacheKey = $"{CacheKeyPrefix}{id}";
        string? cachedUser = await cache.GetStringAsync(cacheKey, ct);
        return cachedUser is null ? null : JsonSerializer.Deserialize<User>(cachedUser);
    }

    private async Task<User?> GetFromDbAsync(Guid id, CancellationToken ct = default)
    {
        ApplicationUser? user = await context.Users.FindAsync([id], ct);
        return user is null ? null : new User { Id = user.Id, Username = user.UserName! };
    }

    private async Task SaveToCacheAsync(User user, CancellationToken ct = default)
    {
        string cacheKey = $"{CacheKeyPrefix}{user.Id}";
        string cachedUser = JsonSerializer.Serialize(user);
        DistributedCacheEntryOptions options = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
        };
        await cache.SetStringAsync(cacheKey, cachedUser, options, ct);
    }
}
