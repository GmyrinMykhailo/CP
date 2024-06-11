using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ActorsApplication
{
    public static class CurrentUser
    {
        public static object currentUser;
    }
    public partial class MainWindow : Window
    {
        private Authentication AuthenticationManager;
        //private User currentUser;
        public MainWindow()
        {
            InitializeComponent();
            AuthenticationManager = new Authentication();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            User user;
            Error error;

            error = AuthenticationManager.Login(LoginInput.Text, PasswordInput.Password.ToString(), out user);
            if (error != null)
            {
                InfoLabel.Background = Brushes.Red;
                InfoLabel.Content = $"{error.message}\n{error.detail}";
                return;
            }
            InfoLabel.Background = Brushes.Green;
            InfoLabel.Content = "Логин прошёл успешно!";

            ThicknessAnimation listAnimation = new ThicknessAnimation();
            listAnimation.From = AuthenticationGrid.Margin;
            listAnimation.To = new Thickness(0, 0, AuthenticationGrid.ActualWidth, 0);
            listAnimation.Duration = TimeSpan.FromSeconds(0.4);
            AuthenticationGrid.BeginAnimation(MarginProperty, listAnimation);

            ConcertsWindow c = new ConcertsWindow(user);
            c.Show();
            this.Close();


        }
    }
}
