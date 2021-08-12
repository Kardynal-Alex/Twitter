using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Twitter.Domain.Exceptions;

namespace Twitter.WebApi.Middlewares
{
    internal sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[Message] : " + ex.Message);
                _logger.LogInformation("[In method] : " + ex.TargetSite);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";

            if (exception is TwitterException)
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            else
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new
            {
                error = exception.Message
            };

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
