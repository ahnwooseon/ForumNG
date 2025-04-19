using System.Text.Json;
using ForumNG.Domain.Interfaces;
using ForumNG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using DomainUser = ForumNG.Domain.Entities.User;
using InfraUser = ForumNG.Infrastructure.Data.User;

namespace ForumNG.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context, IDistributedCache cache) : IUserRepository
{
    private const string CacheKeyPrefix = "users:";

    public async Task<DomainUser?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        DomainUser? user = await GetFromCacheAsync(id, ct);
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

    private async Task<DomainUser?> GetFromCacheAsync(string id, CancellationToken ct = default)
    {
        string cacheKey = $"{CacheKeyPrefix}{id}";
        string? cachedUser = await cache.GetStringAsync(cacheKey, ct);

        return cachedUser is null ? null : JsonSerializer.Deserialize<DomainUser>(cachedUser);
    }

    private async Task<DomainUser?> GetFromDbAsync(string id, CancellationToken ct = default)
    {
        InfraUser? infraUser = await context
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, ct);

        return infraUser is null
            ? null
            : new DomainUser { Id = infraUser.Id, Username = infraUser.UserName! };
    }

    private async Task SaveToCacheAsync(DomainUser user, CancellationToken ct = default)
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
