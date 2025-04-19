using FluentResults;
using ForumNG.Domain.DTOs;
using ForumNG.Domain.Entities;
using ForumNG.Domain.Interfaces;
using Mediator;

namespace ForumNG.Application.Queries.Users.GetUser;

public class GetUserQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserQuery, Result<UserDto>>
{
    public async ValueTask<Result<UserDto>> Handle(GetUserQuery request, CancellationToken ct)
    {
        User? user = await userRepository.GetByIdAsync(request.Id, ct);

        return user is null
            ? Result.Fail<UserDto>($"User with ID {request.Id} not found")
            : Result.Ok(new UserDto(user.Id, user.Username));
    }
}
