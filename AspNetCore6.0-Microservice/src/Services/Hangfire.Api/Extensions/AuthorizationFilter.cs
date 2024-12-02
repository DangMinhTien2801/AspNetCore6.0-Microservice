using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Hangfire.Api.Extensions
{
    public class AuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}
