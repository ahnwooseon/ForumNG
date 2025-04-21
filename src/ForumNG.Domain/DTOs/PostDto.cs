namespace ForumNG.Domain.DTOs;

public record PostDto(Guid Id, Guid AuthorId, Guid TopicId, string Content, DateTime CreatedAt);
