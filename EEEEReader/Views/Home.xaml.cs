using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EEEEReader.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }

        private void HomeFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Impossible de charger la page " +  e.SourcePageType.FullName + " sur HomeFrame");
        }

        // De la doc officielle de microsoft https://learn.microsoft.com/en-us/windows/apps/design/controls/navigationview
        private void NavView_Loaded(object sender, RoutedEventArgs e) 
        {
            // S'abonner à la navigation
            HomeFrame.Navigated += On_Navigated;

            // Page par défaut
            NavView.SelectedItem = NavView.MenuItems[0];
        }

        // De la démo https://github.com/3P3-DevAppWindows-A25/DemoNavigation_Complet/blob/main/DemoNavigation/MainWindow.xaml.cs
        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer != null)
            {
                Type navPageType = Type.GetType(args.SelectedItemContainer.Tag.ToString());
                NavView_Navigate(navPageType, args.RecommendedNavigationTransitionInfo);
            }
        }

        // De la doc officielle de microsoft https://learn.microsoft.com/en-us/windows/apps/design/controls/navigationview
        private void NavView_Navigate(Type navPageType, NavigationTransitionInfo transitionInfo)
        {
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            Type preNavPageType = HomeFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (navPageType is not null && !Type.Equals(preNavPageType, navPageType))
            {
                HomeFrame.Navigate(navPageType, null, transitionInfo);
            }
        }

        // De la doc officielle de microsoft https://learn.microsoft.com/en-us/windows/apps/design/controls/navigationview
        // Sauf le try...catch, qui vient de moi
        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = HomeFrame.CanGoBack;

            // aussi de moi, check si la target est ReadingPage, si oui, la sélection est inutile
            if (e.SourcePageType == typeof(EEEEReader.Views.ReadingPage))
            {
                return;
            }
            else if (HomeFrame.SourcePageType != null)
            {
                try
                {
                    // Select the nav view item that corresponds to the page being navigated to.
                    NavView.SelectedItem = NavView.MenuItems
                                .OfType<NavigationViewItem>()
                                .First(i => i.Tag.Equals(HomeFrame.SourcePageType.FullName.ToString()));
                }
                catch (InvalidOperationException _)
                {
                    NavView.SelectedItem = NavView.FooterMenuItems
                                .OfType<NavigationViewItem>()
                                .First(i => i.Tag.Equals(HomeFrame.SourcePageType.FullName.ToString()));
                }

                NavView.Header =
                    ((NavigationViewItem)NavView.SelectedItem)?.Content?.ToString();

            }
        }

        // De la doc officielle de microsoft https://learn.microsoft.com/en-us/windows/apps/design/controls/navigationview
        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            TryGoBack();
        }

        // De la doc officielle de microsoft https://learn.microsoft.com/en-us/windows/apps/design/controls/navigationview
        private bool TryGoBack()
        {
            if (!HomeFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed.
            if (NavView.IsPaneOpen &&
                (NavView.DisplayMode == NavigationViewDisplayMode.Compact ||
                 NavView.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;

            HomeFrame.GoBack();
            return true;
        }
    }
}
