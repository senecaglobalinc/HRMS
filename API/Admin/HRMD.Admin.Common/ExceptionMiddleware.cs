using HRMS.Admin.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HRMD.Admin.Common
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate m_Next;
        private readonly ILoggerManager m_Logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            m_Logger = logger;
            m_Next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await m_Next(httpContext);
            }
            catch (Exception ex)
            {
                m_Logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error from the custom middleware."
            }.ToString());
        }
    }

}
