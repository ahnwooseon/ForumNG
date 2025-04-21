using FluentResults;
using ForumNG.Domain.DTOs;
using Mediator;

namespace ForumNG.Application.Queries.Users.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<Result<UserDto>>;
