using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HealthcareSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Generate Report at 9PM or 21:00 everyday
            App_Start.ReportScheduler.Instance.ScheduleTask(21, 0, 1,
                () => {
                    // Generate daily report
                    // Check if today is the end of month --> report
                    Controllers.StaffController.GenerateDailyReport();
                    var today = DateTime.Today;
                    if (today.Day == DateTime.DaysInMonth(today.Year, today.Month))
                    {
                        Controllers.StaffController.GenerateMonthlyReport();
                    }
            });
        }
    }
}
