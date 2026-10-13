using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GroupProject
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public ShoeDbContext ShoeDbContext { get; set; }
        public Authorization()
        {
            InitializeComponent();
        }

        private void OpenRegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new Registration();
            registrationWindow.ShowDialog();
        }

        private void AuthorizationButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;

            User? user = ShoeDbContext.Users.Find(login, password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль.");
                return;
            }

            MessageBox.Show($"Добро пожаловать, {user.FirstName ?? user.Login}!");
            // Здесь можно открыть главное окно приложения.
        }
    }
}
