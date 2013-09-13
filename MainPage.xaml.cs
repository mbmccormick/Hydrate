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
        private double goal = 75.0;
        private double current = 0.0;

        private double size = 21.0;

        public MainPage()
        {
            InitializeComponent();

            App.UnhandledExceptionHandled += new EventHandler<ApplicationUnhandledExceptionEventArgs>(App_UnhandledExceptionHandled);

            this.BuildApplicationBar();
        }

        private void BuildApplicationBar()
        {            
        }

        private void App_UnhandledExceptionHandled(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                this.prgLoading.Visibility = System.Windows.Visibility.Visible;
            });
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.IsNavigationInitiator == false)
            {
                LittleWatson.CheckForPreviousException(true);

                LoadData();
            }
        }

        private void LoadData()
        {
            double height = this.vbxBackground.Height;
            double percentage = current / goal;

            double targetHeight = height - (height * percentage);

            if (targetHeight < 0.0)
                targetHeight = 0.0;

            if (targetHeight > height)
                targetHeight = height;

            this.vbxForeground.Height = targetHeight;

            this.txtGoal.Text = Math.Round(goal, 1) + " oz.";
            this.txtCurrent.Text = Math.Round(current, 1) + " oz.";

            if (current >= goal)
            {
                this.txtReminder.Text = "N/A";
                this.vbxComplete.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.txtReminder.Text = "1 hour";
                this.vbxComplete.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        
        private void mnuAdd_Click(object sender, EventArgs e)
        {
            current = current + size;
            LoadData();

            if (current >= goal)
            {
                MessageBox.Show("You have just met your goal of " + goal + " ounces of water for today!", "Success", MessageBoxButton.OK);
            }
        }

        private void mnuSubtract_Click(object sender, EventArgs e)
        {
            current = current - size;
            if (current < 0.0)
                current = 0.0;

            LoadData();
        }

        private void mnuSettings_Click(object sender, EventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
            });
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/YourLastAboutDialog;component/AboutPage.xaml", UriKind.Relative));
            });
        }
    }
}