using FluentResults;
using ForumNG.Domain.DTOs;
using ForumNG.Domain.Entities;
using ForumNG.Domain.Interfaces;
using Mapster;
using Mediator;

namespace ForumNG.Application.Commands.Topics.CreateTopic;

public class CreateTopicCommandHandler(ITopicRepository repository)
    : IRequestHandler<CreateTopicCommand, Result<TopicDto>>
{
    public async ValueTask<Result<TopicDto>> Handle(
        CreateTopicCommand command,
        CancellationToken ct
    )
    {
        Topic topic = new()
        {
            Id = Guid.NewGuid(),
            AuthorId = command.AuthorId,
            Title = command.Title,
            CreatedAt = DateTime.UtcNow,
        };
        await repository.AddAsync(topic, ct);
        TopicDto dto = topic.Adapt<TopicDto>();

        return Result.Ok(dto);
    }
}
