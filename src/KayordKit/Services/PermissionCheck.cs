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
    public async Task PreProcessAsync(IPreProcessorContext context, CancellationToken ct)
    {
        var currentUser = context.HttpContext.Resolve<ICurrentUserService>();
        var logger = context.HttpContext.Resolve<ILogger<PermissionCheck>>();

        // TODO: Check permissions for current call
        logger.LogError($"request:{context.Request?.GetType().FullName} path: {context.HttpContext.Request.Path}");
        logger.LogError($"user:{currentUser.Sub}");

        // TODO: remove this
        await Task.Delay(0);
        // return Task.CompletedTask;
    }
}