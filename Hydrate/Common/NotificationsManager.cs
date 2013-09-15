using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Scheduler;

namespace Hydrate.Common
{
    public class NotificationsManager
    {
        public static void SetupReminders()
        {
            if (ScheduledActionService.Find("BackgroundAgent") == null)
            {
                PeriodicTask task = new PeriodicTask("BackgroundAgent");
                task.Description = "Periodic hydration reminders to keep you motivated towards your daily goal.";

                ScheduledActionService.Add(task);
            }

            if (System.Diagnostics.Debugger.IsAttached)
                ScheduledActionService.LaunchForTest("BackgroundAgent", new TimeSpan(0, 0, 1, 0)); // every minute
        }

        public static void DisableReminders()
        {
            ScheduledActionService.Remove("BackgroundAgent");
        }
    }
}
