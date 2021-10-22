using HidroWebAPI.Util.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace HidroWebAPI.Models.Responses.Http
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool IsMessageUserFriendly { get; set; }

        public ErrorResponse()
        {

        }

        public ErrorResponse(Exception exception)
        {
            this.StatusCode = StatusCodes.Status500InternalServerError;
            this.Message = exception.Message;
        }


        public ErrorResponse(AuthenticationException authenticationException)
        {
            this.StatusCode = StatusCodes.Status401Unauthorized;
            this.Message = authenticationException.Message;
            this.IsMessageUserFriendly = true;
        }

        public ErrorResponse(EntidadeNaoEncontradaException entidadeNaoEncontradaException)
        {
            this.StatusCode = StatusCodes.Status404NotFound;
            this.Message = entidadeNaoEncontradaException.Message;
        }

        public ErrorResponse(RegraDeNegocioException regraDeNegocioException)
        {
            this.StatusCode = StatusCodes.Status500InternalServerError;
            this.Message = regraDeNegocioException.Message;
            this.IsMessageUserFriendly = true;
        }

        public ObjectResult AsObjectResult()
        {
            return new ObjectResult(this)
            {
                StatusCode = this.StatusCode
            };
        }

    }
}
