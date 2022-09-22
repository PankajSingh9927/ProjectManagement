using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ProductManagement.Configration
{
    public class UserAuthActionFilter : Attribute, IActionFilter
    {
        public UserAuthActionFilter()
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var _httpContextAccessor = context?.HttpContext.RequestServices.GetService<IHttpContextAccessor>();
            var userId = context?.HttpContext?.GetUserId()??0;

            if (userId <= 0)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
        }
    }
}
