using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
namespace HealthcareSystem.App_Start
{
    public class ReportScheduler
    {
        private static ReportScheduler _instance;
        private List<Timer> timers = new List<Timer>();

        private ReportScheduler() { }

        public static ReportScheduler Instance => _instance ?? (_instance = new ReportScheduler());

        public void ScheduleTask(int hour, int min, double intervalInDay, Action task)
        {
            var intervalInHour = intervalInDay * 24;
            DateTime now = DateTime.Now;
            DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0);
            if (now > firstRun)
            {
                firstRun = firstRun.AddDays(1);
            }

            TimeSpan timeToGo = firstRun - now;
            if (timeToGo <= TimeSpan.Zero)
            {
                timeToGo = TimeSpan.Zero;
            }

            var timer = new Timer(x =>
            {
                task.Invoke();
            }, null, timeToGo, TimeSpan.FromHours(intervalInHour));

            timers.Add(timer);
        }
    }
}