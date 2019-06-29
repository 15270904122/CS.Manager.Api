using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Manager.Infrastructure.Jobs.HangfireJobExtensions
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
            //var user = context.Request.GetQuery("user");
            //var pwd = context.Request.GetQuery("pwd");
            //if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pwd))
            //{
            //    if (user.Equals(ScheduleSettings.Instance.LoginUser) &&
            //        pwd.Equals(ScheduleSettings.Instance.LoginPassword))
            //    {
            //        return true;
            //    }
            //    return false;
            //}
            //else
            //{
            //    return false;
            //}
        }
    }
}
