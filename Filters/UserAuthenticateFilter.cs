using Microsoft.AspNetCore.Mvc.Filters;
using _2FA.Services;
using _2FA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace _2FA.Filters
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class UserAuthenticate : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string authHeader = context.HttpContext.Request.Headers.Authorization.ToString();
            IActionResult result = null;
            if (authHeader != null && authHeader.StartsWith("Bearer"))
            {
                User user = UserService.UserFromToken(authHeader["Bearer ".Length..].Trim(), false);
                if (user == null)
                    result = new BadRequestResult();
            }
            else
				result = new UnauthorizedResult();
            if (!(context.ActionDescriptor.EndpointMetadata.OfType<IAllowAnonymous>().Any() || result == null))
				context.Result = result;
		}
	}
}