using HidroWebAPI.Models.Responses.Http;
using HidroWebAPI.Util.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HidroWebAPI.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> exceptionHandlers;
        private readonly ILogger logger;

        public ExceptionFilter(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<ExceptionFilter>();
            exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                [typeof(EntidadeNaoEncontradaException)] = HandleEntidadeNaoEncontradaException,
                [typeof(RegraDeNegocioException)] = HandleRegraDeNegocioException
            };
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            LogExceptionContext(context);

            Type exceptionType = context.Exception.GetType();
            if (!exceptionHandlers.ContainsKey(exceptionType))
                HandleUnkownException(context);
            else
                exceptionHandlers[exceptionType](context);

            return Task.CompletedTask;
        }

        private void LogExceptionContext(ExceptionContext context)
        {
            logger.LogError($"An exception occured!{Environment.NewLine}" +
                            $"TraceIdentifier:{context.HttpContext.TraceIdentifier}{Environment.NewLine}" +
                            context.Exception.ToString());
        }

        private void HandleEntidadeNaoEncontradaException(ExceptionContext context)
        {
            EntidadeNaoEncontradaException entidadeNaoEncontradaException =
                context.Exception as EntidadeNaoEncontradaException;

            context.Result = new ErrorResponse(entidadeNaoEncontradaException).AsObjectResult();

            context.ExceptionHandled = true;
        }


        private void HandleRegraDeNegocioException(ExceptionContext context)
        {
            RegraDeNegocioException regraDeNegocioException =
                context.Exception as RegraDeNegocioException;

            context.Result = new ErrorResponse(regraDeNegocioException).AsObjectResult();

            context.ExceptionHandled = true;
        }

        private void HandleUnkownException(ExceptionContext context)
        {
            context.Result = new ErrorResponse(context.Exception).AsObjectResult();

            context.ExceptionHandled = true;
        }
    }
}
