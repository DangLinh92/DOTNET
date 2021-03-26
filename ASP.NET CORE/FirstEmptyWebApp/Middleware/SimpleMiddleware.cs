using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FirstEmptyWebApp.Middleware
{
    public class SimpleMiddleware
    {
        private readonly RequestDelegate _next;
        public SimpleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
           await context.Response.WriteAsync("<div>Hello simple middleware</div>");
           await _next(context);
           await context.Response.WriteAsync("<div>Hello simple middleware - back</div>");
        }
    }
}
