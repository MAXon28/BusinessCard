using BusinessCard.BusinessLogicLayer.Utils.Exceptions;
using BusinessCard.BusinessLogicLayer.Utils.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BusinessCard.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (MAXonValidationException validationEx)
            {
                await HandleValidationExceptionAsync(context, validationEx);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleValidationExceptionAsync(HttpContext context, MAXonValidationException validationException)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.BadRequest;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                TypeOfError = validationException.ValidationType.ToStringAttribute(),
                ErrorMessage = validationException.Message
            });
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">  </param>
        /// <param name="exception">  </param>
        /// <returns>  </returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = "Неопределённая ошибка"
            });
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}