using Hangfire.Dashboard;

namespace Police.Web {

    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter {

        public bool Authorize(DashboardContext context) => context.GetHttpContext()?.User?.Identity?.IsAuthenticated ?? false;

    }

}