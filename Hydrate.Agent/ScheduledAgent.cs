using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Hydrate.Common;
using System;
using Microsoft.Phone.Shell;

namespace Hydrate.Agent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private int startHour = 9;
        private int endHour = 20;

        static ScheduledAgent()
        {
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        protected override void OnInvoke(ScheduledTask task)
        {
            if (DateTime.Now.Hour >= startHour &&
                DateTime.Now.Hour < endHour)
            {
                AppSettings settings = new AppSettings();

                if (settings.Current < settings.Goal &&
                    settings.LastReminder.AddHours(settings.Reminder) <= DateTime.Now)
                {
                    ShellToast toast = new ShellToast();

                    toast.Title = "Hydrate";
                    toast.Content = "Don't forget to stay hydrated!";
                    toast.NavigationUri = new Uri("/MainPage.xaml", UriKind.Relative);

                    toast.Show();

                    settings.LastReminder = DateTime.Now;
                }
            }

            if (System.Diagnostics.Debugger.IsAttached)
                ScheduledActionService.LaunchForTest("BackgroundAgent", new TimeSpan(0, 0, 1, 0)); // every minute

            NotifyComplete();
        }
    }
}