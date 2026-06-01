using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace SampleWebApi.UserHealthPings
{
    public class UserHealthFilter : IAuthorizationFilter
    {
        UserHealthPing _user;
        public UserHealthFilter(UserHealthPing user)
        {
            this._user = user;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var username = context.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!_user.Lives.TryGetValue(username, out _))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
