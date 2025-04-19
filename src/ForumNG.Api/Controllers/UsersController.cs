using FluentResults;
using ForumNG.Application.Queries.Users.GetUser;
using ForumNG.Domain.DTOs;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace ForumNG.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetById(Guid id, CancellationToken ct)
    {
        Result<UserDto> result = await mediator.Send(new GetUserQuery(id.ToString()), ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(
                new ProblemDetails
                {
                    Title = "User not found",
                    Detail = result.Errors.First().Message,
                }
            );
    }
}
