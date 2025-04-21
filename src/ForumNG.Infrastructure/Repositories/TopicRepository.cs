using ForumNG.Domain.Entities;
using ForumNG.Domain.Interfaces;
using ForumNG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ForumNG.Infrastructure.Repositories;

public class TopicRepository(ApplicationDbContext context) : ITopicRepository
{
    public async Task AddAsync(Topic topic, CancellationToken ct)
    {
        await context.Topics.AddAsync(topic, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<Topic?> GetByIdAsync(Guid id, CancellationToken ct) =>
        await context.Topics.FindAsync([id], ct);

    public async Task<List<Topic>> GetAllAsync(CancellationToken ct) =>
        await context
            .Topics.Include(t => t.Posts)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(ct);
}
