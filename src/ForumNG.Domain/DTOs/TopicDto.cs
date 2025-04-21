namespace ForumNG.Domain.DTOs;

public record TopicDto(Guid Id, Guid AuthorId, string Title, DateTime CreatedAt);
