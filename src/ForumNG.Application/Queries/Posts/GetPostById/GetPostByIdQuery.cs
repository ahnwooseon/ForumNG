using FluentResults;
using ForumNG.Domain.DTOs;
using Mediator;

namespace ForumNG.Application.Queries.Posts.GetPostById;

public record GetPostByIdQuery(Guid Id) : IRequest<Result<PostDto>>;
