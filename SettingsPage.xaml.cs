using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Hydrate.Common;
using Windows.Networking.Proximity;
using Windows.Storage.Streams;

namespace Hydrate
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        private ProximityDevice proximityDevice = null;
        
        public SettingsPage()
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
                this.prgLoading.Visibility = System.Windows.Visibility.Collapsed;
            });
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            this.txtGoal.Text = App.Settings.Goal.ToString();
            this.txtSize.Text = App.Settings.Size.ToString();
            this.lstReminder.SelectedIndex = App.Settings.Reminder;
        }

        private void mnuDone_Click(object sender, EventArgs e)
        {
            App.Settings.Goal = Convert.ToDouble(this.txtGoal.Text);
            App.Settings.Size = Convert.ToDouble(this.txtSize.Text);
            App.Settings.Reminder = this.lstReminder.SelectedIndex;

            NavigationService.GoBack();
        }

        private void writeNFC_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tap your NFC tag to begin setting it up for use with Hydrate.", "Write NFC tag", MessageBoxButton.OK);

            this.prgLoading.Visibility = System.Windows.Visibility.Visible;

            proximityDevice = ProximityDevice.GetDefault();
            proximityDevice.SubscribeForMessage("WriteableTag", OnWriteableTagArrived);
        }

        private void OnWriteableTagArrived(ProximityDevice sender, ProximityMessage message)
        {
            var dataWriter = new DataWriter();
            dataWriter.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf16LE;

            string appLauncher = string.Format("hydrate:newRecord");
            dataWriter.WriteString(appLauncher);

            long messageId = sender.PublishBinaryMessage("WindowsUri:WriteTag", dataWriter.DetachBuffer());
            sender.StopPublishingMessage(messageId);

            this.prgLoading.Visibility = System.Windows.Visibility.Collapsed;

            MessageBox.Show("Your NFC tag has been setup for use with Hydrate.", "Success", MessageBoxButton.OK);
        }
    }
}