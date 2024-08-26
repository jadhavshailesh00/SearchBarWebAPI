using Microsoft.AspNetCore.Authorization;

namespace Interview.App_Start.Handler
{
    public class UserAuthorizationHandler : AuthorizationHandler<DeveloperRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DeveloperRequirement requirement)
        {
            if (context.User.IsInRole("Developer"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class DeveloperRequirement : IAuthorizationRequirement
    {
    }
}
