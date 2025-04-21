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

        var dto = topic.Adapt<TopicDto>();
        return Result.Ok(dto);
    }
}
