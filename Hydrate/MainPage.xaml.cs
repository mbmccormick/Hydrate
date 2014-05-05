using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Hydrate.API;
using Hydrate.API.Models;
using System.Collections.ObjectModel;
using Hydrate.Common;
using Microsoft.Phone.Tasks;
using System.Windows.Media;
using Windows.ApplicationModel.Store;

namespace Hydrate
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region List Properties

        public static ObservableCollection<Post> TopPosts { get; set; }
        public static ObservableCollection<Post> NewPosts { get; set; }
        public static ObservableCollection<Post> AskPosts { get; set; }

        #endregion

        private bool isTopLoaded = false;
        private bool isNewLoaded = false;
        private bool isAskLoaded = false;

        public MainPage()
        {
            InitializeComponent();

            App.UnhandledExceptionHandled += new EventHandler<ApplicationUnhandledExceptionEventArgs>(App_UnhandledExceptionHandled);

            TopPosts = new ObservableCollection<Post>();
            NewPosts = new ObservableCollection<Post>();
            AskPosts = new ObservableCollection<Post>();
        }

        private void App_UnhandledExceptionHandled(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                ToggleLoadingText();
                ToggleEmptyText();

                this.prgLoading.Visibility = System.Windows.Visibility.Collapsed;
            });
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.IsNavigationInitiator == false)
            {
                LittleWatson.CheckForPreviousException(true);

                if (isTopLoaded == false ||
                    isNewLoaded == false ||
                    isAskLoaded == false)
                {
                    LoadData();
                }

                FeedbackHelper.PromptForRating();
            }
        }

        private async void LoadData()
        {
            this.prgLoading.Visibility = System.Windows.Visibility.Visible;

            await App.HydrateClient.GetTopPosts((result) =>
            {
                SmartDispatcher.BeginInvoke(() =>
                {
                    TopPosts.Clear();

                    foreach (Post item in result)
                    {
                        TopPosts.Add(item);
                    }

                    isTopLoaded = true;

                    if (isTopLoaded &&
                        isNewLoaded &&
                        isAskLoaded)
                    {
                        ToggleLoadingText();
                        ToggleEmptyText();

                        this.prgLoading.Visibility = System.Windows.Visibility.Collapsed;
                    }
                });
            });

            await App.HydrateClient.GetNewPosts((result) =>
            {
                SmartDispatcher.BeginInvoke(() =>
                {
                    NewPosts.Clear();

                    foreach (Post item in result)
                    {
                        NewPosts.Add(item);
                    }

                    isNewLoaded = true;

                    if (isTopLoaded &&
                        isNewLoaded &&
                        isAskLoaded)
                    {
                        ToggleLoadingText();
                        ToggleEmptyText();

                        this.prgLoading.Visibility = System.Windows.Visibility.Collapsed;
                    }
                });
            });

            await App.HydrateClient.GetAskPosts((result) =>
            {
                SmartDispatcher.BeginInvoke(() =>
                {
                    AskPosts.Clear();

                    foreach (Post item in result)
                    {
                        AskPosts.Add(item);
                    }

                    isAskLoaded = true;

                    if (isTopLoaded &&
                        isNewLoaded &&
                        isAskLoaded)
                    {
                        ToggleLoadingText();
                        ToggleEmptyText();

                        this.prgLoading.Visibility = System.Windows.Visibility.Collapsed;
                    }
                });
            });
        }

        private void ToggleLoadingText()
        {
            this.txtTodayLoading.Visibility = System.Windows.Visibility.Collapsed;
            this.txtHistoryLoading.Visibility = System.Windows.Visibility.Collapsed;

            //this.lstTopPosts.Visibility = System.Windows.Visibility.Visible;
            //this.lstNewPosts.Visibility = System.Windows.Visibility.Visible;
        }

        private void ToggleEmptyText()
        {
            //if (TopPosts.Count == 0)
            //    this.txtTopPostsEmpty.Visibility = System.Windows.Visibility.Visible;
            //else
            //    this.txtTopPostsEmpty.Visibility = System.Windows.Visibility.Collapsed;

            //if (NewPosts.Count == 0)
            //    this.txtNewPostsEmpty.Visibility = System.Windows.Visibility.Visible;
            //else
            //    this.txtNewPostsEmpty.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            if (this.prgLoading.Visibility == System.Windows.Visibility.Visible) return;

            isTopLoaded = false;
            isNewLoaded = false;
            isAskLoaded = false;

            LoadData();
        }

        private void Feedback_Click(object sender, EventArgs e)
        {
            if (this.prgLoading.Visibility == System.Windows.Visibility.Visible) return;

            FeedbackHelper.Default.Feedback();
        }

        private async void Donate_Click(object sender, EventArgs e)
        {
            try
            {
                var productList = await CurrentApp.LoadListingInformationAsync();
                var product = productList.ProductListings.FirstOrDefault(p => p.Value.ProductType == ProductType.Consumable);
                var receipt = await CurrentApp.RequestProductPurchaseAsync(product.Value.ProductId, true);

                if (CurrentApp.LicenseInformation.ProductLicenses[product.Value.ProductId].IsActive)
                {
                    CurrentApp.ReportProductFulfillment(product.Value.ProductId);

                    MessageBox.Show("Thank you for your donation! Your support motivates me to keep developing for Hacker News, the best Hacker News client for Windows Phone.", "Thank You", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        private void About_Click(object sender, EventArgs e)
        {
            if (this.prgLoading.Visibility == System.Windows.Visibility.Visible) return;

            NavigationService.Navigate(new Uri("/YourLastAboutDialog;component/AboutPage.xaml", UriKind.Relative));
        }
    }
}