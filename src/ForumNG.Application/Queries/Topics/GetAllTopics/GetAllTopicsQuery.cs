using FluentResults;
using ForumNG.Domain.DTOs;
using Mediator;

namespace ForumNG.Application.Queries.Topics.GetAllTopics;

public record GetAllTopicsQuery() : IRequest<Result<List<TopicDto>>>;
