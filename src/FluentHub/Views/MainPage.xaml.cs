using FluentHub.Helpers;
using FluentHub.Services;
using FluentHub.Views.Home;
using Microsoft.Extensions.DependencyInjection;
using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FluentHub.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            CoreApplication.GetCurrentView().TitleBar.LayoutMetricsChanged += TitleBar_LayoutMetricsChanged;
            navigationService = App.Current.Services.GetService<INavigationService>();
        }
        private readonly INavigationService navigationService;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationService.Configure(TabView, MainFrame, typeof(UserHomePage));
            navigationService.Navigate<UserHomePage>();

            // Configure Jumplist
            await JumpListHelper.ConfigureDefaultJumpListAsync();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            navigationService.Disconnect();
        }

        private void TitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            CoreApplication.GetCurrentView().TitleBar.LayoutMetricsChanged += OnTitleBarLayoutMetricsChanged;
        }

        private void OnTitleBarLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
            => RightPaddingColumn.Width = new GridLength(sender.SystemOverlayRightInset);

        private void DragArea_Loaded(object sender, RoutedEventArgs e) => Window.Current.SetTitleBar(DragArea);

        private void HomeButton_Click(object sender, RoutedEventArgs e) => navigationService.Navigate<UserHomePage>();
        private void GoBack() => navigationService.GoBack();
        private void GoForward() => navigationService.GoForward();
        private async void ShareWithBrowserMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var currentItem = navigationService.TabView.SelectedItem.NavigationHistory.CurrentItem;
            var result = await Windows.System.Launcher.LaunchUriAsync(new Uri(currentItem.Url));
            System.Diagnostics.Debug.WriteLine("LaunchUriAsync({0}) - result:{1}", currentItem.Url, result);
        }

        private void SettingsMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            navigationService.Navigate<AppSettings.MainSettingsPage>();
        }

        private void SignOutMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            // Temporary treatment (Deletion requested credentials must be deleted)
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(SignIn.IntroPage));
        }
    }
}