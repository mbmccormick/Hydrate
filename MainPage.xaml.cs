using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using Hydrate.Common;
using Microsoft.Phone.Tasks;
using System.Windows.Media;

namespace Hydrate
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            App.UnhandledExceptionHandled += new EventHandler<ApplicationUnhandledExceptionEventArgs>(App_UnhandledExceptionHandled);
        }

        private void App_UnhandledExceptionHandled(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
            {
                this.prgLoading.Visibility = System.Windows.Visibility.Collapsed;
            });
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.IsNavigationInitiator == false)
            {
                LittleWatson.CheckForPreviousException(true);
            }

            LoadData();

            if (NavigationContext.QueryString.ContainsKey("newRecord") == true)
            {
                mnuAdd_Click(this, e);
            }
        }

        private void LoadData()
        {
            this.prgLoading.Visibility = System.Windows.Visibility.Visible;

            double height = this.vbxBackground.Height;
            double percentage = App.Settings.Current / App.Settings.Goal;

            double targetHeight = height - (height * percentage);

            if (targetHeight < 0.0)
                targetHeight = 0.0;

            if (targetHeight > height)
                targetHeight = height;

            this.vbxForeground.Height = targetHeight;

            this.txtGoal.Text = Math.Round(App.Settings.Goal, 1) + " oz.";
            this.txtCurrent.Text = Math.Round(App.Settings.Current, 1) + " oz.";

            if (App.Settings.Reminder == 1)
            {
                this.txtReminder.Text = "1 hour";
            }
            else if (App.Settings.Reminder == 2)
            {
                this.txtReminder.Text = "2 hours";
            }
            else
            {
                this.txtReminder.Text = "N/A";
            }

            if (App.Settings.Current >= App.Settings.Goal)
            {
                this.txtReminder.Text = "N/A";
                this.vbxComplete.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.vbxComplete.Visibility = System.Windows.Visibility.Collapsed;
            }

            RemindersManager.ClearReminders();

            if (App.Settings.Reminder > 0 &&
                App.Settings.Current < App.Settings.Goal)
            {
                RemindersManager.SetupReminders();
            }

            this.prgLoading.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void mnuAdd_Click(object sender, EventArgs e)
        {
            if (this.prgLoading.Visibility == System.Windows.Visibility.Visible) return;

            App.Settings.Current += App.Settings.Size;
            LoadData();
        }

        private void mnuSubtract_Click(object sender, EventArgs e)
        {
            if (this.prgLoading.Visibility == System.Windows.Visibility.Visible) return;

            App.Settings.Current -= App.Settings.Size;
            if (App.Settings.Current < 0.0)
                App.Settings.Current = 0.0;

            LoadData();
        }

        private void mnuSettings_Click(object sender, EventArgs e)
        {
            if (this.prgLoading.Visibility == System.Windows.Visibility.Visible) return;

            SmartDispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
            });
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            if (this.prgLoading.Visibility == System.Windows.Visibility.Visible) return;

            SmartDispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/YourLastAboutDialog;component/AboutPage.xaml", UriKind.Relative));
            });
        }
    }
}