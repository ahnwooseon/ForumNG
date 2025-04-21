using FluentResults;
using ForumNG.Domain.DTOs;
using Mediator;

namespace ForumNG.Application.Queries.Topics.GetTopicById;

public record GetTopicByIdQuery(Guid Id) : IRequest<Result<TopicDto>>;
