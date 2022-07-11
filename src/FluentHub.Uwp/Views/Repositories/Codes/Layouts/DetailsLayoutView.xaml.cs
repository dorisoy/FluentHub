﻿using FluentHub.Uwp.Models;
using FluentHub.Uwp.Services;
using FluentHub.Uwp.Services.Navigation;
using FluentHub.Uwp.ViewModels;
using FluentHub.Uwp.ViewModels.Repositories;
using FluentHub.Uwp.ViewModels.Repositories.Codes;
using FluentHub.Uwp.ViewModels.Repositories.Codes.Layouts;
using Microsoft.Extensions.DependencyInjection;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using muxc = Microsoft.UI.Xaml.Controls;

namespace FluentHub.Uwp.Views.Repositories.Codes.Layouts
{
    public sealed partial class DetailsLayoutView : Page
    {
        public DetailsLayoutView()
        {
            InitializeComponent();

            var provider = App.Current.Services;
            ViewModel = provider.GetRequiredService<DetailsLayoutViewModel>();
            navigationService = App.Current.Services.GetRequiredService<INavigationService>();
        }

        private static Repository RepositoryCache { get; set; }
        public DetailsLayoutViewModel ViewModel { get; }
        private readonly INavigationService navigationService;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var url = e.Parameter as string;
            var uri = new Uri(url);
            var pathSegments = uri.AbsolutePath.Split("/").ToList();
            pathSegments.RemoveAt(0);

            if (RepositoryCache is null || RepositoryCache?.Name != pathSegments[1])
            {
                // Load repository info
                var command1 = ViewModel.LoadRepositoryCommand;
                if (command1.CanExecute(url))
                    await command1.ExecuteAsync(url);

                RepositoryCache = ViewModel.Repository;
            }

            bool isRootDir = false;
            bool isFile = false;
            bool isSubDir = false;
            bool isDir = false;
            string path = "";
            string branchName = "";

            // URL has root path and default branch
            if (pathSegments.Count() == 2) isRootDir = true;
            // URL has different branch name and/or repository content path
            if (pathSegments.Count() > 2)
            {
                isFile = pathSegments[2] == "blob" ? true : false;
                isDir = pathSegments[2] == "tree" ? true : false;

                branchName = pathSegments[3];

                if (pathSegments.Count() == 4) isRootDir = true;

                // URL has path
                if (pathSegments.Count() > 4)
                {
                    pathSegments.RemoveRange(0, 4);
                    path = string.Join("/", pathSegments);
                    if (isDir) isSubDir = true;
                }
            }
            else
            {
                branchName = RepositoryCache.DefaultBranchName;
                path = null;
            }

            ViewModel.ContextViewModel = new()
            {
                Repository = RepositoryCache,

                BranchName = branchName,
                IsDir = isDir,
                IsFile = isFile,
                IsSubDir = isSubDir,
                IsRootDir = isRootDir,
                Path = path,
            };

            #region tabitem
            var currentItem = navigationService.TabView.SelectedItem.NavigationHistory.CurrentItem;

            if (ViewModel.ContextViewModel.IsRootDir)
            {
                if (string.IsNullOrEmpty(ViewModel.ContextViewModel.Repository.Description))
                    currentItem.Header = $"{ViewModel.ContextViewModel.Repository.Owner.Login}/{ViewModel.ContextViewModel.Repository.Name}";
                else
                    currentItem.Header = $"{ViewModel.ContextViewModel.Repository.Owner.Login}/{ViewModel.ContextViewModel.Repository.Name}: {ViewModel.ContextViewModel.Repository.Description}";
            }
            else
                currentItem.Header = $"{ViewModel.ContextViewModel.Repository.Name}/{ViewModel.ContextViewModel.Path} at {ViewModel.ContextViewModel.BranchName} · {ViewModel.ContextViewModel.Repository.Owner.Login}/{ViewModel.ContextViewModel.Repository.Name}";

            currentItem.Url = url;
            currentItem.Description = currentItem.Header;

            string displayurl;
            if (ViewModel.ContextViewModel.IsRootDir)
            {
                if (ViewModel.ContextViewModel.Repository.DefaultBranchName == ViewModel.ContextViewModel.BranchName)
                    displayurl = $"{ViewModel.ContextViewModel.Repository.Owner.Login} / {ViewModel.ContextViewModel.Repository.Name}";
                else
                    displayurl = $"{ViewModel.ContextViewModel.Repository.Owner.Login} / {ViewModel.ContextViewModel.Repository.Name} / {ViewModel.ContextViewModel.BranchName}";
            }
            else displayurl = $"{ViewModel.ContextViewModel.Repository.Owner.Login} / {ViewModel.ContextViewModel.Repository.Name} / {ViewModel.ContextViewModel.BranchName} / {string.Join(" / ", ViewModel.ContextViewModel.Path.Split("/"))}";

            currentItem.DisplayUrl = displayurl;

            currentItem.Icon = new muxc.ImageIconSource
            {
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Icons/Repositories.png"))
            };
            #endregion

            var command2 = ViewModel.RefreshDetailsLayoutPageCommand;
            if (command2.CanExecute(url))
                command2.Execute(url);
        }

        private void OnDirListViewDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var item = DirListView.SelectedItem as DetailsLayoutListViewModel;
            var tagItem = item?.ObjectTag?.Split("/");

            string path = ViewModel.ContextViewModel.Path;
            if (!string.IsNullOrEmpty(path)) path += "/";

            path += tagItem.Last();

            string objType = tagItem.First() == "blob" ? "blob" : "tree";

            string url = $"{App.DefaultGitHubDomain}/{ViewModel.ContextViewModel.Repository.Owner.Login}/{ViewModel.ContextViewModel.Repository.Name}/{objType}/{ViewModel.ContextViewModel.BranchName}/{path}";

            MainPageViewModel.RepositoryContentFrame.Navigate(typeof(DetailsLayoutView), url);
        }

        private void OnLatestReleaseClick(object sender, RoutedEventArgs e)
        {
            MainPageViewModel.RepositoryContentFrame.Navigate(typeof(ReleasesPage), ViewModel.ContextViewModel);
        }
    }
}