using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using demo2209.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace demo2209;

public partial class PeopleWindow : Window
{
    private string _fio;
    private string _role;
    private Bitmap? _image;

    public class Enters
    {
        public DateTime? Time { get; set; }
        public string UserLogin { get; set; }
        public string LoginType { get; set; }
    }

    private List<Enters> _enters;


    public PeopleWindow()
    {
        InitializeComponent();
    }

    public PeopleWindow(string fio, string role, Bitmap? image)
    {
        InitializeComponent();
       

        _fio = fio;
        _role = role;
        _image = image;

        imageBox.Source = _image;
        fioBox.Text = _fio;
        roleBox.Text = _role;

        LoadData();
    }

    private void LoadData()
    {
        using var context = new DanyaContext();
        _enters = context.LoginHistories
            .Select(s => new Enters
        {
            Time = s.LoginTime,
            UserLogin = s.Employee.Login,
            LoginType = s.LoginType

        }).ToList();

        EnterBox.ItemsSource = _enters;
    }
}