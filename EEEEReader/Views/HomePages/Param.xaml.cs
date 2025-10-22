using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EEEEReader.Views.HomePages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Param : Page
{
    public Param()
    {
        InitializeComponent();
        LoadThemePreference();
    }

    private void LoadThemePreference()
    {
        // True si dark est active
        ThemeToggle.IsOn = App.AppReader.CurrentTheme == ElementTheme.Dark;
    }

    private void ThemeToggle_Toggled(object sender, RoutedEventArgs e)
    {
        ElementTheme theme = ThemeToggle.IsOn ? ElementTheme.Dark : ElementTheme.Light;
        App.ChangeTheme(theme);
    }

    private void Deconnection(object sender, RoutedEventArgs e)
    {
        if (App.MainWindow?.Content is Frame mainFrame)
        {
            App.AppReader.CurrentUser = null;
            mainFrame.Navigate(typeof(LoginPage));

        }
    }
}
