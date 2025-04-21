using FluentResults;
using ForumNG.Domain.DTOs;
using ForumNG.Domain.Entities;
using ForumNG.Domain.Interfaces;
using Mapster;
using Mediator;

namespace ForumNG.Application.Queries.Topics.GetAllTopics;

public class GetAllTopicsQueryHandler(ITopicRepository repository)
    : IRequestHandler<GetAllTopicsQuery, Result<List<TopicDto>>>
{
    public async ValueTask<Result<List<TopicDto>>> Handle(
        GetAllTopicsQuery request,
        CancellationToken ct
    )
    {
        List<Topic> topics = await repository.GetAllAsync(ct);
        var dtos = topics.Adapt<List<TopicDto>>();
        return Result.Ok(dtos);
    }
}
