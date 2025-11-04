using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using demo2209.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace demo2209;

public partial class OrderWindow : Window
{
    private List<Client> _clients;

    public class Client
    {
        public string FioClient { get; set; }
    }
    

    public OrderWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        using var context = new DanyaContext();

        //_clients = context.Clients.Select(c => new Client
        //{
        //  FioClient = c.Fio,
        //}).ToList();
        //ClientsComboBox.ItemsSource = _clients;
        ClientsComboBox.ItemsSource = context.Clients.Select(e => e.Fio).ToList();
        ServiceComboBox.ItemsSource = context.Services.Select(c => c.ServiceName).ToList();

        int orderCount = context.Orders.Count()+1;
        OrderNumberTextBox.Text = orderCount.ToString();

    }

    private void SearchClient_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }

    private void AddClient_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var Client = new AddClient();
        Client.Show();
        this.Close();
    }

    private void AddService_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }

    private void ViewClients_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }

    private void SaveToPdf_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }

    private void SaveOrder_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
}