using FluentResults;
using ForumNG.Domain.DTOs;
using Mediator;

namespace ForumNG.Application.Commands.Topics.CreateTopic;

public record CreateTopicCommand(string CategoryName, Guid AuthorId, string Title, string Content)
    : IRequest<Result<TopicDto>>;
