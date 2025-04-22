using FluentResults;
using ForumNG.Domain.DTOs;
using ForumNG.Domain.Entities;
using ForumNG.Domain.Interfaces;
using Mapster;
using Mediator;

namespace ForumNG.Application.Queries.Topics.GetPostsByTopicId;

public class GetPostsByTopicIdQueryHandler(IPostRepository repository)
    : IRequestHandler<GetPostsByTopicIdQuery, Result<List<PostDto>>>
{
    public async ValueTask<Result<List<PostDto>>> Handle(
        GetPostsByTopicIdQuery request,
        CancellationToken ct
    )
    {
        List<Post> posts = await repository.GetByTopicIdAsync(request.Id, ct);
        return Result.Ok(posts.Adapt<List<PostDto>>());
    }
}
