using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace EEEEReader.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            string username = UserNameBox.Text;
            string password = PasswordBox.Password;

            
            if (username == "e" && password == "e")
            {
                this.Frame?.Navigate(typeof(Home));
            }
            else
            {
                return; // TODO clear et message d'erreur
            }
        }
    }
}