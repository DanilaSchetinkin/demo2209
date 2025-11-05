using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using demo2209.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace demo2209;

public partial class OrderWindow : Window
{
    
    private List<Service> _allServices;
    private ObservableCollection<SelectedService> _selectedServices;

     

    public class SelectedService
    {
        public string ServiceName { get; set; }
        public string ServiceCost { get; set; }
        public int ServiceId { get; set; }
    }

    public OrderWindow()
    {
        InitializeComponent();
        LoadData();
        
    }

    private void LoadData()
    {
        using var context = new DanyaContext();

        ClientsComboBox.ItemsSource = context.Clients.Select(e => e.Fio).ToList();

        _allServices = context.Services.ToList();
        ServiceComboBox.ItemsSource = _allServices.Select(c => c.ServiceName).ToList();

        _selectedServices = new ObservableCollection<SelectedService>();
        SelectedServicesListBox.ItemsSource = _selectedServices;

        int orderCount = context.Orders.Count() + 1;
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
        string selectedServiceName = ServiceComboBox.SelectedItem as string;

        if (!string.IsNullOrEmpty(selectedServiceName))
        {
            // Находим полную информацию об услуге из базы данных
            var selectedService = _allServices.FirstOrDefault(s => s.ServiceName == selectedServiceName);

            if (selectedService != null)
            {
                // Создаем новую выбранную услугу
                var newSelectedService = new SelectedService
                {
                    ServiceName = selectedService.ServiceName,
                    ServiceCost = $"{selectedService.Cost} руб." // Предполагая, что у Service есть свойство Cost
                };

                // Добавляем в коллекцию
                _selectedServices.Add(newSelectedService);
                SelectedServicesListBox.ItemsSource = new ObservableCollection<SelectedService>(_selectedServices);
                // Обновляем общую стоимость

            }
        }
    }

    // Метод для обновления общей стоимости
    //private void UpdateTotalCost()
    //{
    //    decimal totalCost = _selectedServices.Sum(service => service.);
    //    TotalCostTextBlock.Text = $"{totalCost} руб.";
    //}



    private void ViewClients_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }

    private void SaveToPdf_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }

    private void SaveOrder_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new DanyaContext();
        var order = new Order()
        {
            Id = context.Orders.OrderBy(o => o.Id).LastOrDefault().Id + 1,
            CodeOrder = OrderNumberTextBox.Text,
            DateCreate = DateTime.Now,
            TimeOrder = OrderNumberTextBox.Text,
            CodeClient = int.Parse(OrderNumberTextBox.Text),
            Status = "Новая",
            DateClose = null,
            TimeRental = "60 Минут",
            EmployeeId = Class1.idUser 
        };

        context.Orders.Add(order);
        context.SaveChanges();

        foreach (var item in _selectedServices)
        {
           
            var service = new Service()
            {
                Id = context.Orders.OrderBy(o => o.Id).LastOrDefault().Id + 1,
                ServiceId = context.Services.OrderBy(x => x.ServiceId).LastOrDefault().ServiceId + 1
            };

            order.Idservices.Add(service);
            context.SaveChanges();
        }

    }
}