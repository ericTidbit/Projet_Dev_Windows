using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using EEEEReader.Models;
using EEEEReader.Views;

namespace EEEEReader
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static Window? MainWindow { get; private set; }
        public static Appli AppReader { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            AppReader = new Appli();
            LoadSavedTheme();
        }

        private void LoadSavedTheme()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.TryGetValue("AppTheme", out object themeValue))
            {
                AppReader.CurrentTheme = (string)themeValue == "Dark" 
                    ? ElementTheme.Dark 
                    : ElementTheme.Light;
            }
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            //creation d'une frame qui est mise dans l'app 
            var m_window = new MainWindow();
            Frame rootFrame = new Frame();
            rootFrame.NavigationFailed += OnNavigationFailed;
                   
            rootFrame.RequestedTheme = AppReader.CurrentTheme;
            
            rootFrame.Navigate(typeof(LoginPage), args.Arguments);
            m_window.Content = rootFrame;
            MainWindow = m_window;
            m_window.Activate();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        public static void ChangeTheme(ElementTheme theme)
        {
            AppReader.CurrentTheme = theme;
            
            // Apply theme to the window's content Frame
            if (MainWindow?.Content is Frame frame)
            {
                frame.RequestedTheme = theme;
            }
        }
    }
}
