using Microsoft.AspNetCore.Authorization;

namespace Interview.App_Start.Handler
{
    public class AdminAuthorizationHandler : AuthorizationHandler<AdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class AdminRequirement : IAuthorizationRequirement
    {
    }
}
