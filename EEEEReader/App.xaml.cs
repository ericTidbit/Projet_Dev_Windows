using EEEEReader.Data;
using EEEEReader.Models;
using EEEEReader.ViewModels;
using EEEEReader.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using EEEEReader.Data;

namespace EEEEReader
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static Window? MainWindow { get; private set; }
        public static Appli AppReader { get; private set; }

        public App()
        {
            InitializeComponent();
            AppReader = new Appli();
            LoadSavedTheme();
        }

        // Load the saved theme from local settings and set AppReader.IsDarkMode
        private void LoadSavedTheme()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (localSettings.Values.TryGetValue("AppTheme", out object themeValue))
            {
                var themeString = themeValue as string;

                // true if "Dark", otherwise false (Light)
                AppReader.IsDarkMode = string.Equals(themeString, "Dark", StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                // Default to light if nothing saved
                AppReader.IsDarkMode = false;
            }
        }

        // Convert boolean to ElementTheme
        private ElementTheme GetActualTheme()
        {
            return AppReader.IsDarkMode ? ElementTheme.Dark : ElementTheme.Light;
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            InitialiserBaseDeDonnees();
            // Create the main window and root frame
            var m_window = new MainWindow();
            Frame rootFrame = new Frame();
            rootFrame.NavigationFailed += OnNavigationFailed;

            // Apply theme to the frame
            rootFrame.RequestedTheme = GetActualTheme();

            // Navigate to login page
            rootFrame.Navigate(typeof(LoginPage), args.Arguments);
            m_window.Content = rootFrame;
            MainWindow = m_window;
            m_window.Activate();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Change the theme at runtime using a boolean.
        /// isDark = true  -> Dark theme
        /// isDark = false -> Light theme
        /// </summary>
        public static void ChangeTheme(bool isDark)
        {
            AppReader.IsDarkMode = isDark;

            ElementTheme theme = isDark ? ElementTheme.Dark : ElementTheme.Light;

            // Save the preference
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["AppTheme"] = isDark ? "Dark" : "Light";

            // Apply theme to the window's content Frame
            if (MainWindow?.Content is Frame frame)
            {
                frame.RequestedTheme = theme;
            }
        }
        private void InitialiserBaseDeDonnees()
        {
            using EEEEReaderDbContext context = new EEEEReaderDbContext();
            DataSeeder seeder = new DataSeeder(context);
            seeder.Seed();
        }
    }
}
