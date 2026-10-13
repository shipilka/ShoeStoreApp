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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public ShoeDbContext ShoeDbContext { get; set; }
        public Registration()
        {
            InitializeComponent();
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            if (ShoeDbContext.Users.Contains(new User { Login = login }))
            {
                MessageBox.Show("Пользователь с таким логином уже существует.");
                return;
            }

            ShoeDbContext.Add(new User
            {
                Login = login,
                Password = password,
                LastName = LastNameTextBox.Text.Trim(),
                FirstName = FirstNameTextBox.Text.Trim(),
                Patronymic = PatronymicTextBox.Text.Trim()
            });

            MessageBox.Show("Регистрация прошла успешно.");
            DialogResult = true;
            Close();
        }
    }
}
