using ForumNG.Application.Queries.Users.GetUserById;
using Mediator;

namespace ForumNG.Api.Endpoints;

public static class UsersEndpoints
{
    public static void MapUsersEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(
            ApiEndpoints.Users.Get,
            (Guid id, ISender mediator, CancellationToken ct) =>
                MinimalApiHelpers.HandleResult(
                    mediator.Send(new GetUserByIdQuery(id), ct),
                    Results.Ok,
                    Results.NotFound
                )
        );
    }
}
