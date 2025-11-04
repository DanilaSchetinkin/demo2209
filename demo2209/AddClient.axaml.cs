using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using demo2209.Models;
using System;
using System.Linq;

namespace demo2209;

public partial class AddClient : Window
{
    public AddClient()
    {
        InitializeComponent();
    }

    private void Add_Client(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

        try
        {
            using var context = new DanyaContext();

            int idCount = context.Clients.Count() + 1;

            var newClient = new Client()
            {
                Id = idCount,
                CodeCliend = long.Parse(CodeBox.Text),
                Email = emailBox.Text,
                Fio = FioBox.Text,
                Address = adresBox.Text,
                Passport = passportBox.Text,
                Password = passportBox.Text,
                Birthday = DateTime.Parse(BirthDayBox.Text)
            };

            context.Clients.Add(newClient);
            context.SaveChanges();

            OrderWindow orderWindow = new OrderWindow();
            orderWindow.Show();
            this.Close();
        }
        catch(Exception ex)
        {
            ShowErrorDialog("Ошибка", $"Произошла ошибка\n{ex}");
        }
        
    }

    private void ShowErrorDialog(string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Content = new TextBlock { Text = message },
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        dialog.ShowDialog(this);
    }

}