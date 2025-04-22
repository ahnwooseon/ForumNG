using FluentResults;
using ForumNG.Domain.DTOs;
using ForumNG.Domain.Entities;
using ForumNG.Domain.Interfaces;
using Mapster;
using Mediator;

namespace ForumNG.Application.Queries.Topics.GetTopicById;

public class GetTopicByIdQueryHandler(ITopicRepository repository)
    : IRequestHandler<GetTopicByIdQuery, Result<TopicDto>>
{
    public async ValueTask<Result<TopicDto>> Handle(GetTopicByIdQuery request, CancellationToken ct)
    {
        Topic? topic = await repository.GetByIdAsync(request.Id, ct);

        if (topic is null)
            return Result.Fail<TopicDto>("Topic not found");

        /*TopicDto result = new(
            topic.Id,
            topic.AuthorId,
            topic.Title,
            topic.Posts?.Count ?? 0,
            topic.Posts?.Max(p => p.CreatedAt) ?? DateTime.MinValue
        );*/

        var result = topic.Adapt<TopicDto>();
        return Result.Ok(result);
    }
}
