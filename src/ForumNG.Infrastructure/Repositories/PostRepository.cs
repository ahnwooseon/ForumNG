using ForumNG.Domain.Entities;
using ForumNG.Domain.Interfaces;
using ForumNG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ForumNG.Infrastructure.Repositories;

public class PostRepository(ApplicationDbContext context) : IPostRepository
{
    public async Task AddAsync(Post post, CancellationToken ct)
    {
        await context.Posts.AddAsync(post, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<Post?> GetByIdAsync(Guid id, CancellationToken ct) =>
        await context.Posts.FindAsync([id], ct);

    public async Task<List<Post>> GetByTopicIdAsync(Guid topicId, CancellationToken ct) =>
        await context.Posts.Where(p => p.TopicId == topicId).ToListAsync(ct);
}
