using ForumNG.Application.Commands.Topics.CreateTopic;
using ForumNG.Application.Queries.Categories.GetTopicsByCategoryName;
using Mediator;

namespace ForumNG.Api.Endpoints;

public static class CategoriesEndpoints
{
    public static void MapCategoriesEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost(
            ApiEndpoints.Categories.PostTopic,
            (CreateTopicCommand cmd, ISender mediator, CancellationToken ct) =>
                MinimalApiHelpers.HandleResult(
                    mediator.Send(cmd, ct),
                    topic => Results.Created($"/api/topics/{topic.Id}", topic),
                    err => Results.Problem(err)
                )
        );

        app.MapGet(
            ApiEndpoints.Categories.GetTopics,
            (string categoryName, ISender mediator, CancellationToken ct) =>
                MinimalApiHelpers.HandleResult(
                    mediator.Send(new GetTopicsByCategoryNameQuery(categoryName), ct),
                    Results.Ok,
                    Results.NotFound
                )
        );
    }
}
