using FluentResults;
using ForumNG.Domain.DTOs;
using Mediator;

namespace ForumNG.Application.Queries.Users.GetUser;

public record GetUserQuery(string Id) : IRequest<Result<UserDto>>;
