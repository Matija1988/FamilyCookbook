using FamilyCookbook.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Common.Exceptions.Handler
{
    public sealed class CustomExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            (string Detail, string Title, int StatusCode) details = exception switch
            {
                InvalidServerException =>
                (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
                BadRequestException =>
                (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                NotFoundException =>
                (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound
                ),
                ModelValidationException => (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                _ =>
                (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
                
            };
            var problemDetails = new ProblemDetails
            {
                Title = details.Title,
                Detail = details.Detail,
                Status = details.StatusCode,
                Instance = httpContext.Request.Path
            };
            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

            if(exception is ModelValidationException validationException)
            {
                problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
            }

            httpContext.Response.StatusCode = details.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails);
            return true;
            
        }
    }
}
