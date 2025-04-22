namespace ForumNG.Domain.Entities;

public class Topic
{
    public Guid Id { get; set; }
    public required Guid CategoryId { get; set; }
    public required Guid AuthorId { get; set; }
    public required string Title { get; set; }
    public required DateTime CreatedAt { get; set; }

    // Navigation property
    public List<Post> Posts { get; set; } = [];
}
