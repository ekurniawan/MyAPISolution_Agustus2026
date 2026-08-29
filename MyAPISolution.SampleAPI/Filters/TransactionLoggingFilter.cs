using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog.Context;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MyAPISolution.SampleAPI.Filters
{
    /// <summary>
    /// Logs every controller action invocation (transaction/audit log):
    /// who called it, the HTTP request (method, path, query, arguments), and the response
    /// (status code + JSON result payload), plus the outcome (success/error + duration).
    /// Registered globally in Program.cs via options.Filters.Add&lt;TransactionLoggingFilter&gt;().
    /// </summary>
    public class TransactionLoggingFilter : IAsyncActionFilter
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
            PropertyNamingPolicy = null,
            WriteIndented = false
        };

        private readonly ILogger<TransactionLoggingFilter> _logger;

        public TransactionLoggingFilter(ILogger<TransactionLoggingFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            var actionName = context.RouteData.Values["action"]?.ToString();
            var userName = context.HttpContext.User?.Identity?.IsAuthenticated == true
                ? (context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? context.HttpContext.User.Identity!.Name)
                : "Anonymous";
            var traceId = context.HttpContext.TraceIdentifier;
            var method = request.Method;
            var path = request.Path.ToString();
            var queryString = request.QueryString.HasValue ? request.QueryString.Value : null;

            // Sensitive values (password, secret, token, authorization, etc.) are masked inside SafeSerialize.
            var requestJson = SafeSerialize(context.ActionArguments);

            using (LogContext.PushProperty("TraceId", traceId))
            using (LogContext.PushProperty("User", userName))
            using (LogContext.PushProperty("Controller", controllerName))
            using (LogContext.PushProperty("Action", actionName))
            {
                var stopwatch = Stopwatch.StartNew();

                _logger.LogInformation(
                    "Action started: {Method} {Path}{QueryString} [{Controller}/{Action}] by {User} | Request: {RequestJson}",
                    method, path, queryString, controllerName, actionName, userName, requestJson);

                try
                {
                    var executedContext = await next();
                    stopwatch.Stop();

                    if (executedContext.Exception != null && !executedContext.ExceptionHandled)
                    {
                        _logger.LogError(
                            executedContext.Exception,
                            "Action failed: {Method} {Path} [{Controller}/{Action}] by {User} in {ElapsedMilliseconds}ms",
                            method, path, controllerName, actionName, userName, stopwatch.ElapsedMilliseconds);
                    }
                    else
                    {
                        var statusCode = (executedContext.Result as Microsoft.AspNetCore.Mvc.Infrastructure.IStatusCodeActionResult)?.StatusCode
                            ?? context.HttpContext.Response.StatusCode;
                        var responseJson = ExtractResponseJson(executedContext.Result);

                        _logger.LogInformation(
                            "Action completed: {Method} {Path} [{Controller}/{Action}] by {User} -> {StatusCode} in {ElapsedMilliseconds}ms | Response: {ResponseJson}",
                            method, path, controllerName, actionName, userName, statusCode, stopwatch.ElapsedMilliseconds, responseJson);
                    }
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(
                        ex,
                        "Action threw an unhandled exception: {Method} {Path} [{Controller}/{Action}] by {User} in {ElapsedMilliseconds}ms",
                        method, path, controllerName, actionName, userName, stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        private static string ExtractResponseJson(IActionResult? result)
        {
            object? value = result switch
            {
                ObjectResult objectResult => objectResult.Value,
                ContentResult contentResult => contentResult.Content,
                _ => null
            };

            if (value == null)
            {
                return result?.GetType().Name ?? "(no result)";
            }

            return SafeSerialize(value);
        }

        private static string SafeSerialize(object? value)
        {
            try
            {
                var node = JsonSerializer.SerializeToNode(value, SerializerOptions);
                RedactSensitiveNodes(node);
                return node?.ToJsonString(SerializerOptions) ?? "null";
            }
            catch (Exception ex)
            {
                return $"(unable to serialize: {ex.Message})";
            }
        }

        /// <summary>
        /// Recursively walks a JSON node tree and replaces the value of any property whose
        /// name looks sensitive (password, secret, token, authorization, apikey) with a masked placeholder.
        /// </summary>
        private static void RedactSensitiveNodes(JsonNode? node)
        {
            switch (node)
            {
                case JsonObject jsonObject:
                    foreach (var key in jsonObject.Select(p => p.Key).ToList())
                    {
                        if (IsSensitiveKey(key))
                        {
                            jsonObject[key] = "***REDACTED***";
                        }
                        else
                        {
                            RedactSensitiveNodes(jsonObject[key]);
                        }
                    }
                    break;
                case JsonArray jsonArray:
                    foreach (var item in jsonArray)
                    {
                        RedactSensitiveNodes(item);
                    }
                    break;
            }
        }

        private static bool IsSensitiveKey(string propertyName)
        {
            var name = propertyName.ToLowerInvariant();
            return name.Contains("password")
                || name.Contains("secret")
                || name.Contains("token")
                || name.Contains("authorization")
                || name.Contains("apikey")
                || name.Contains("api_key");
        }
    }
}

