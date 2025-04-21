using FluentResults;

namespace ForumNG.Api.Endpoints;

public static class MinimalApiHelpers
{
    public static async Task<IResult> HandleResult<T>(
        ValueTask<Result<T>> resultTask,
        Func<T, IResult> onSuccess,
        Func<string?, IResult?> onFailure,
        string? errorTitle = null,
        int errorStatusCode = 400,
        string? errorType = null
    )
    {
        Result<T> result = await resultTask;
        if (result.IsSuccess)
            return onSuccess(result.Value);

        IResult? failureResult = onFailure?.Invoke(result.Errors.FirstOrDefault()?.Message);
        return failureResult ?? Results.Problem();

    }
}
