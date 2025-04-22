namespace ForumNG.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    // Navigation property
    public List<Topic> Topics { get; set; } = [];
}
