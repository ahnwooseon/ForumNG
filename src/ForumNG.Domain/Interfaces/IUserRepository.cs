using ForumNG.Domain.Entities;

namespace ForumNG.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
}
