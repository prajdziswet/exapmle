using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MyApp.Class
{
    public class ChangeMaxAge
    {
        readonly RequestDelegate _next;
        public ChangeMaxAge(RequestDelegate _next)
        {
            this._next = _next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path;
            if (path == "/Home/Send")
            {
                await _next.Invoke(context);
                context.Response.Headers.Add("Cache-Control", "public,max-age=1");
            }
            else await _next.Invoke(context);
        }
    }
}
