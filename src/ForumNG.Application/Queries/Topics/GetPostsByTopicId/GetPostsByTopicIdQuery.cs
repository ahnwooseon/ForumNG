using FluentResults;
using ForumNG.Domain.DTOs;
using Mediator;

namespace ForumNG.Application.Queries.Topics.GetPostsByTopicId;

public record GetPostsByTopicIdQuery(Guid Id) : IRequest<Result<List<PostDto>>>;
