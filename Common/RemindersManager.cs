using Microsoft.Phone.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydrate.Common
{
    public class RemindersManager
    {
        private static int expirationTime = 20;
        
        public static void ClearReminders()
        {
            foreach (var item in ScheduledActionService.GetActions<Reminder>())
            {
                ScheduledActionService.Remove(item.Name);
            }
        }

        public static void SetupReminders()
        {
            int currentHour = DateTime.Now.Hour + App.Settings.Reminder;

            while (currentHour < expirationTime)
            {
                Reminder reminder = new Reminder("newRecordReminder" + i);

                reminder.Title = "Hydration Reminder";
                reminder.Content = "Don't forget to keep drinking water, your goal for today is " + App.Settings.Goal + " ounces!";
                reminder.BeginTime = DateTime.Now.AddHours(currentHour);
                reminder.NavigationUri = new Uri("/MainPage.xaml", UriKind.Relative);

                ScheduledActionService.Add(reminder);

                currentHour += App.Settings.Reminder;
            }
        }
    }
}
