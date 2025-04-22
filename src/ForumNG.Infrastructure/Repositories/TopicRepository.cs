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
        await context.Topics.Include(t => t.Posts)
            .FirstOrDefaultAsync(t => t.Id == id, ct);// ([topicId], ct);

    public async Task<List<Topic>> GetByCategoryNameAsync(string categoryName, CancellationToken ct)
    {
        Guid categoryId = await context.Categories
            .Where(c => c.Name == categoryName)
            .Select(c => c.Id)
            .FirstOrDefaultAsync(ct);

        return categoryId == Guid.Empty
            ? []
            : await context.Topics
                .Where(t => t.CategoryId == categoryId)
                .Include(t => t.Posts)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(ct);
    }
    /*
        await context
            .Topics.Where(t => t.CategoryName == categoryName)
            .Include(t => t.Posts)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(ct);
*/
    public async Task<List<Topic>> GetAllAsync(CancellationToken ct) =>
        await context
            .Topics.Include(t => t.Posts)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(ct);
}
