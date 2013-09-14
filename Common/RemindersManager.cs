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
            for (int i = 1; i <= (expirationTime - DateTime.Now.Hour); i++)
            {
                Reminder reminder = new Reminder("newRecordReminder" + i);

                reminder.Title = "Hydration Reminder";
                reminder.Content = "Don't forget to stay hydrated, your goal for today is " + App.Settings.Goal + " ounces!";
                reminder.BeginTime = DateTime.Now.AddHours(i * App.Settings.Reminder);
                reminder.NavigationUri = new Uri("/MainPage.xaml", UriKind.Relative);

                ScheduledActionService.Add(reminder);
            }
        }
    }
}
