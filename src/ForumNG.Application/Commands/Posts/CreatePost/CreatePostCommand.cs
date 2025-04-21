using FluentResults;
using ForumNG.Domain.DTOs;
using Mediator;

namespace ForumNG.Application.Commands.Posts.CreatePost;

public record CreatePostCommand(Guid AuthorId, Guid TopicId, string Content)
    : IRequest<Result<PostDto>>;
