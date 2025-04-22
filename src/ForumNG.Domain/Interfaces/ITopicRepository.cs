using ForumNG.Domain.Entities;

namespace ForumNG.Domain.Interfaces;

public interface ITopicRepository
{
    Task AddAsync(Topic topic, CancellationToken ct);

    Task<Topic?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<List<Topic>> GetByCategoryNameAsync(string categoryName, CancellationToken ct);

    Task<List<Topic>> GetAllAsync(CancellationToken ct);
}
