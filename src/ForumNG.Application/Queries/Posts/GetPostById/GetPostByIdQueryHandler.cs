using FluentResults;
using ForumNG.Domain.DTOs;
using ForumNG.Domain.Entities;
using ForumNG.Domain.Interfaces;
using Mapster;
using Mediator;

namespace ForumNG.Application.Queries.Posts.GetPostById;

public class GetPostByIdQueryHandler(IPostRepository postRepository)
    : IRequestHandler<GetPostByIdQuery, Result<PostDto>>
{
    public async ValueTask<Result<PostDto>> Handle(GetPostByIdQuery request, CancellationToken ct)
    {
        Post? post = await postRepository.GetByIdAsync(request.Id, ct);

        if (post is null)
            return Result.Fail<PostDto>("Post not found");

        var dto = post.Adapt<PostDto>();
        return Result.Ok(dto);
    }
}
