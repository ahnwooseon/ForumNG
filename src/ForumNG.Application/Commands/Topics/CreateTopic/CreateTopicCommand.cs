using FluentResults;
using ForumNG.Domain.DTOs;
using Mediator;

namespace ForumNG.Application.Commands.Topics.CreateTopic;

public record CreateTopicCommand(Guid AuthorId, string Title) : IRequest<Result<TopicDto>>;
