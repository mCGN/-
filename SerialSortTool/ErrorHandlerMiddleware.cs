using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialSortTool
{
    public class ErrorHandlerMiddleware
    {
        public readonly RequestDelegate next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                await Log(context, e);
                throw;
            }
        }

        public async Task Log(HttpContext context,Exception e)
        {
            string msg = "系统异常";
            int ErrorCode = 1000;
            var result = new
            {
                success = false,
                msg = msg,
                realMsg = e.Message,
                code = ErrorCode,
            };
            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result), Encoding.UTF8);
        }

    }
}
