using ForumNG.Application.Commands.Topics.CreateTopic;
using ForumNG.Application.Queries.Topics.GetAllTopics;
using ForumNG.Application.Queries.Topics.GetTopicById;
using Mediator;

namespace ForumNG.Api.Endpoints;

public static class TopicsEndpoints
{
    public static void MapTopicsEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost(
            ApiEndpoints.Topics.Create,
            (CreateTopicCommand cmd, ISender mediator, CancellationToken ct) =>
                MinimalApiHelpers.HandleResult(
                    mediator.Send(cmd, ct),
                    topic => Results.Created($"/api/topics/{topic.Id}", topic),
                    err => Results.Problem(err)
                )
        );

        app.MapGet(
            ApiEndpoints.Topics.Get,
            (Guid id, ISender mediator, CancellationToken ct) =>
                MinimalApiHelpers.HandleResult(
                    mediator.Send(new GetTopicByIdQuery(id), ct),
                    Results.Ok,
                    Results.NotFound
                )
        );

        app.MapGet(
            ApiEndpoints.Topics.GetAll,
            (ISender mediator, CancellationToken ct) =>
                MinimalApiHelpers.HandleResult(
                    mediator.Send(new GetAllTopicsQuery(), ct),
                    Results.Ok,
                    err => Results.Problem(err)
                )
        );
    }
}
