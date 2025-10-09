using Avalonia.Controls;
using demo2209.Models;
using System.Linq;

namespace demo2209
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            string log = login.Text;
            string pass = password.Text;
            
            ErrorBox.Text = "";

            if (string.IsNullOrEmpty(log))
            {
                ShowError("Введите логин");
                return;
            }

            if (string.IsNullOrEmpty(pass))
            {
                ShowError("Введите пароль");
                return;
            }

            using var context = new DanyaContext();

            var user = context.Employees
                .FirstOrDefault(e => e.Login == log && e.Password == pass);

            if (user == null)
            {
                ErrorBox.Text = "Неверный логин или пароль";
                return;
            }
        }

        private void ShowError(string message, bool isError = true)
        {
            ErrorBox.Text = message;
            ErrorBox.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red);
        }

    }
}