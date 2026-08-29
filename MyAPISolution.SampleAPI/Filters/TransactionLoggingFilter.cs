using Microsoft.AspNetCore.Mvc.Filters;
using Serilog.Context;
using System.Diagnostics;
using System.Security.Claims;

namespace MyAPISolution.SampleAPI.Filters
{
    /// <summary>
    /// Logs every controller action invocation (transaction/audit log):
    /// who called it, which action + arguments, and the outcome (success/error + duration).
    /// Registered globally in Program.cs via options.Filters.Add&lt;TransactionLoggingFilter&gt;().
    /// </summary>
    public class TransactionLoggingFilter : IAsyncActionFilter
    {
        private readonly ILogger<TransactionLoggingFilter> _logger;

        public TransactionLoggingFilter(ILogger<TransactionLoggingFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            var actionName = context.RouteData.Values["action"]?.ToString();
            var userName = context.HttpContext.User?.Identity?.IsAuthenticated == true
                ? (context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? context.HttpContext.User.Identity!.Name)
                : "Anonymous";
            var traceId = context.HttpContext.TraceIdentifier;

            // Redact sensitive arguments (password, secret, token) before logging.
            var arguments = context.ActionArguments
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => IsSensitive(kvp.Key) ? "***REDACTED***" : kvp.Value);

            using (LogContext.PushProperty("TraceId", traceId))
            using (LogContext.PushProperty("User", userName))
            using (LogContext.PushProperty("Controller", controllerName))
            using (LogContext.PushProperty("Action", actionName))
            {
                var stopwatch = Stopwatch.StartNew();

                _logger.LogInformation(
                    "Action started: {Controller}/{Action} by {User} with arguments {@Arguments}",
                    controllerName, actionName, userName, arguments);

                try
                {
                    var executedContext = await next();
                    stopwatch.Stop();

                    if (executedContext.Exception != null && !executedContext.ExceptionHandled)
                    {
                        _logger.LogError(
                            executedContext.Exception,
                            "Action failed: {Controller}/{Action} by {User} in {ElapsedMilliseconds}ms",
                            controllerName, actionName, userName, stopwatch.ElapsedMilliseconds);
                    }
                    else
                    {
                        var statusCode = (executedContext.Result as Microsoft.AspNetCore.Mvc.Infrastructure.IStatusCodeActionResult)?.StatusCode
                            ?? context.HttpContext.Response.StatusCode;

                        _logger.LogInformation(
                            "Action completed: {Controller}/{Action} by {User} -> {StatusCode} in {ElapsedMilliseconds}ms",
                            controllerName, actionName, userName, statusCode, stopwatch.ElapsedMilliseconds);
                    }
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(
                        ex,
                        "Action threw an unhandled exception: {Controller}/{Action} by {User} in {ElapsedMilliseconds}ms",
                        controllerName, actionName, userName, stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        private static bool IsSensitive(string parameterName)
        {
            var name = parameterName.ToLowerInvariant();
            return name.Contains("password") || name.Contains("secret") || name.Contains("token");
        }
    }
}
