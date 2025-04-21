using FluentResults;
using ForumNG.Domain.DTOs;
using ForumNG.Domain.Entities;
using ForumNG.Domain.Interfaces;
using Mapster;
using Mediator;

namespace ForumNG.Application.Commands.Posts.CreatePost;

public class CreatePostCommandHandler(IPostRepository repository)
    : IRequestHandler<CreatePostCommand, Result<PostDto>>
{
    public async ValueTask<Result<PostDto>> Handle(CreatePostCommand command, CancellationToken ct)
    {
        Post post = new()
        {
            Id = Guid.NewGuid(),
            AuthorId = command.AuthorId,
            TopicId = command.TopicId,
            Content = command.Content,
            CreatedAt = DateTime.UtcNow,
        };
        await repository.AddAsync(post, ct);
        PostDto dto = post.Adapt<PostDto>();

        return Result.Ok(dto);
    }
}
