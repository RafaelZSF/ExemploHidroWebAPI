using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mime;

namespace HidroWebAPI.Controllers
{
    //[Authorize]
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public abstract class ApiController : ControllerBase
    {
        private IMediator mediator;
        protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
