using FluentResults;
using ForumNG.Domain.DTOs;
using Mediator;

namespace ForumNG.Application.Queries.Categories.GetTopicsByCategoryName;

public record GetTopicsByCategoryNameQuery(string CategoryName) : IRequest<Result<List<TopicDto>>>;
