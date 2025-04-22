using FluentResults;
using ForumNG.Domain.DTOs;
using ForumNG.Domain.Entities;
using ForumNG.Domain.Interfaces;
using Mapster;
using Mediator;

namespace ForumNG.Application.Queries.Categories.GetTopicsByCategoryName;

public class GetTopicsByCategoryNameQueryHandler(ITopicRepository repository)
    : IRequestHandler<GetTopicsByCategoryNameQuery, Result<List<TopicDto>>>
{
    public async ValueTask<Result<List<TopicDto>>> Handle(
        GetTopicsByCategoryNameQuery request,
        CancellationToken ct
    )
    {
        List<Topic> topics = await repository.GetByCategoryNameAsync(request.CategoryName, ct);

        /*
        List<TopicDto> result = topics.Select(t => new TopicDto(
            t.Id,
            t.AuthorId,
            t.Title,
            t.Posts?.Count ?? 0,
            //t.Posts.OrderByDescending(p => p.CreatedAt).FirstOrDefault()?.CreatedAt ?? DateTime.MinValue
            t.Posts.Max(p => p.CreatedAt)
        )).ToList();
        */

        var result = topics.Adapt<List<TopicDto>>();
        return Result.Ok(result);
    }
}
