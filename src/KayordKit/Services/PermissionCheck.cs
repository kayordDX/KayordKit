using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace KayordKit.Services;

// public class PermissionCheck<TRequest> : IPreProcessor<TRequest>
// {
//     public Task PreProcessAsync(TRequest req, HttpContext ctx, List<ValidationFailure> failures, CancellationToken ct)
//     {
//         var logger = ctx.Resolve<ILogger<TRequest>>();

//         logger.LogInformation($"request:{req?.GetType().FullName} path: {ctx.Request.Path}");

//         return Task.CompletedTask;
//     }
// }

/* 
This will check path and method
Example:
    /dataProvider GET
    Check current user id roles and permissions for this endpoint
*/

public class PermissionCheck : IGlobalPreProcessor
{
    public async Task PreProcessAsync(object req, HttpContext ctx, List<ValidationFailure> failures, CancellationToken ct)
    {
        var currentUser = ctx.Resolve<ICurrentUserService>();
        var logger = ctx.Resolve<ILogger<PermissionCheck>>();

        // TODO: Check permissions for current call
        logger.LogError($"request:{req?.GetType().FullName} path: {ctx.Request.Path}");
        logger.LogError($"user:{currentUser.Sub}");

        // return Task.CompletedTask;
    }
}