using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VacationTracking.Domain.Exceptions;
using ValidationException = FluentValidation.ValidationException;

namespace VacationTracking.Api.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (VacationTrackingException ex)
            {
                await HandleCustomExceptionAsync(context, ex);
            }

            catch (ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleCustomExceptionAsync(HttpContext context, VacationTrackingException exception)
        {
            var response = context.Response;
            response.StatusCode = exception.Code;
            response.ContentType = "application/json";

            await response.WriteAsync(JsonConvert.SerializeObject(new CustomErrorResponse
            {
                Message = exception.Message,
                Description = exception.Description
            }));
        }

        private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = 400;

            var errors = exception.Errors.Select(e => new
            {
                e.PropertyName,
                e.ErrorMessage
            }).ToList();

            var errorText = JsonConvert.SerializeObject(errors);

            await response.WriteAsync(errorText);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // TODO: Log exception
            var response = context.Response;
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = exception.Message;
            var description = "Unexpected error";

            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(new CustomErrorResponse
            {
                Message = message,
                Description = description
            }));
        }
    }
}
