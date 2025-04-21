using FluentResults;
using ForumNG.Domain.DTOs;
using Mediator;

namespace ForumNG.Application.Queries.Posts.GetPostsByTopicId;

public record GetPostsByTopicIdQuery(Guid TopicId) : IRequest<Result<List<PostDto>>>;
