using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace HidroWebAPI.Utils
{
    public static class ExceptionHandlerEvents
    {
        public static async Task OnExceptionAsync(HttpContext context)
        {
            Exception exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (exception != null)
            {
                object responseBody = new
                {
                    exception.Message
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                byte[] responseBodyAsByteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseBody));
                await context.Response.Body.WriteAsync(responseBodyAsByteArray, 0, responseBodyAsByteArray.Length);
            }
        }
    }
}
