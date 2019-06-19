using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Manager.Api.Authorization
{
    public class ApiAuthorizationFilter : ActionFilterAttribute
    {
        //
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {

            }
            else if (context.HttpContext.Response != null)
            {
            }
            else
            {
                await base.OnActionExecutionAsync(context, next);
            }
        }

    }
}
