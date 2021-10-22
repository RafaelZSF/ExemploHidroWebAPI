using HidroWebAPI.Models.Responses.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HidroWebAPI.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                ModelStateDictionary modelStateDictionary = context.ModelState;
                Dictionary<string, List<string>> errors = modelStateDictionary.ToDictionary
                    (modelStateEntryByPropName => modelStateEntryByPropName.Key.Replace("$.", ""),
                     modelStateEntryByPropName => modelStateEntryByPropName.Value.Errors.Select(e => e.ErrorMessage).ToList());
                context.Result = new BadRequestObjectResult(new BadRequestResponse() { Errors = errors });
                return;
            }

            await next();
        }
    }
}
