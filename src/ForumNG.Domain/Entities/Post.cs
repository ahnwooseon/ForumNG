namespace ForumNG.Domain.Entities;

public class Post
{
    public Guid Id { get; set; }
    public required Guid AuthorId { get; set; }
    public required Guid TopicId { get; set; }
    public required string Content { get; set; }
    public required DateTime CreatedAt { get; set; }

    // Navigation property
    public Topic? Topic { get; set; }
}
