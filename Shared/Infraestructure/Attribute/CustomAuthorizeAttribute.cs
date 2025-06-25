using learning_center_back.Security.Domai_.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace learning_center_back.Shared.Infraestructure.Attribute;

public class CustomAuthorizeAttribute : System.Attribute, IAsyncAuthorizationFilter
{
    private readonly string[] _roles;

    public CustomAuthorizeAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Items["User"] as User; //User1 role mkt

        if (user == null || !_roles[0].Contains(user.Role))
        {
            context.Result = new ForbidResult();
        }
    }

}