
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace FilmsAPI.Filters
{
    public class RoleAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public RoleAuthorizationFilter(string role)
        {
            _role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!(user.Identity?.IsAuthenticated ?? false) || !user.IsInRole(_role))
            {
                context.Result = new ForbidResult(); // Trả về lỗi 403 Forbidden
            }
        }
    }
}
