using JobManagementApi.Models.Exceptions;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JobManagementApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BaseRequestException e)
            {
                context.Response.StatusCode = e.StatusCode;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(e.Message);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"{e.Message}");
            }

        }
    }
}
