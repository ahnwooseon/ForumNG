using ForumNG.Domain.Entities;

namespace ForumNG.Domain.Interfaces;

public interface IPostRepository
{
    Task AddAsync(Post post, CancellationToken ct);

    Task<Post?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<List<Post>> GetByTopicIdAsync(Guid id, CancellationToken ct);
}
