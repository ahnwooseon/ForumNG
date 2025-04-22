using ForumNG.Application.Commands.Posts.CreatePost;
using ForumNG.Application.Queries.Posts.GetPostById;
using Mediator;

namespace ForumNG.Api.Endpoints;

public static class PostsEndpoints
{
    public static void MapPostsEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost(
            ApiEndpoints.Posts.Create,
            (CreatePostCommand cmd, ISender mediator, CancellationToken ct) =>
                MinimalApiHelpers.HandleResult(
                    mediator.Send(cmd, ct),
                    post => Results.Created($"/api/posts/{post.Id}", post),
                    err => Results.Problem(err)
                )
        );

        app.MapGet(
            ApiEndpoints.Posts.Get,
            (Guid id, ISender mediator, CancellationToken ct) =>
                MinimalApiHelpers.HandleResult(
                    mediator.Send(new GetPostByIdQuery(id), ct),
                    Results.Ok,
                    Results.NotFound
                )
        );
    }
}
