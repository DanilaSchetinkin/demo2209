using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using demo2209.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace demo2209;

public partial class PeopleWindow : Window
{
    private string _fio;
    private string _role;
    private Bitmap? _image;
    private Timer _sessionTimer;
    private int _sessionTimeSeconds;
    private bool _isWarningShown = false;
    private DateTime _sessionStartTime;

    private const int TOTAL_SESSION_TIME_MINUTES = 10;
    private const int WARNING_TIME_MINUTES = 5;
    private const int BLOCK_TIME_MINUTES = 3;

    // Класс для хранения настроек блокировки
    private class BlockSettings
    {
        public DateTime BlockUntil { get; set; }
    }

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

        LoginComboBox.SelectionChanged += Sort_ComboBox_Login;
        DateComboBox.SelectionChanged += Sort_ComboBox_Date;

        SessionTimer();
    }

    private void SessionTimer()
    {
        _sessionStartTime = DateTime.Now;
        _sessionTimeSeconds = TOTAL_SESSION_TIME_MINUTES * 60;

        _sessionTimer = new Timer(SessionTimerCallback, null, 0, 1000);
    }

    private void SessionTimerCallback(object state)
    {
        Dispatcher.UIThread.Post(() =>
        {
            _sessionTimeSeconds--;

            var minutes = _sessionTimeSeconds / 60;
            var seconds = _sessionTimeSeconds % 60;
            SessionTimerText.Text = $"Время сеанса: {minutes:00}:{seconds:00}";

            CheckSessionConditions();
        });
    }

    private void CheckSessionConditions()
    {
        if (_sessionTimeSeconds <= WARNING_TIME_MINUTES * 60 && !_isWarningShown)
        {
            ShowWarningMessage();
            _isWarningShown = true;
        }

        if (_sessionTimeSeconds <= 0)
        {
            EndSession();
        }
    }

    private void ShowWarningMessage()
    {
        WarningText.Text = $"ВНИМАНИЕ! Сеанс завершится через {WARNING_TIME_MINUTES} минут!";
        WarningText.IsVisible = true;

        var dialog = new Window()
        {
            Title = "Предупреждение",
            Content = new TextBlock
            {
                Text = $"До окончания сеанса осталось {WARNING_TIME_MINUTES} минут!\nПодготовьтесь к завершению работы.",
                Margin = new Thickness(20),
                TextWrapping = Avalonia.Media.TextWrapping.Wrap
            },
            Width = 300,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        dialog.ShowDialog(this);
    }

    private async void EndSession()
    {
        _sessionTimer?.Dispose();
        SaveSessionEndInfo();

        var dialog = new Window()
        {
            Title = "Сеанс завершен",
            Content = new StackPanel
            {
                Children =
                {
                    new TextBlock
                    {
                        Text = "Время сеанса истекло!",
                        Margin = new Thickness(20),
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                    },
                    new TextBlock
                    {
                        Text = $"Система заблокирована на {BLOCK_TIME_MINUTES} минут.",
                        Margin = new Thickness(20),
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                    }
                }
            },
            Width = 350,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        await dialog.ShowDialog(this);
        CloseWindowAndBlockSystem();
    }

    private void SaveSessionEndInfo()
    {
        try
        {
            using var context = new DanyaContext();
            var sessionEndRecord = new LoginHistory
            {
                LoginTime = _sessionStartTime,
                Employee = context.Employees.FirstOrDefault(e => e.Login == _fio),
                LoginType = "Автоматическое завершение по таймеру"
            };
            context.LoginHistories.Add(sessionEndRecord);
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка сохранения информации о сеансе: {ex.Message}");
        }
    }

    private void CloseWindowAndBlockSystem()
    {
        var blockUntil = DateTime.Now.AddMinutes(BLOCK_TIME_MINUTES);
        SaveBlockTime(blockUntil);
        Close();
    }

    private void SaveBlockTime(DateTime blockUntil)
    {
        try
        {
            var settings = new BlockSettings { BlockUntil = blockUntil };
            var json = JsonSerializer.Serialize(settings);
            File.WriteAllText("block_settings.json", json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка сохранения времени блокировки: {ex.Message}");
        }
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        _sessionTimer?.Dispose();
        base.OnClosing(e);
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

    private void Sort_ComboBox_Login(object sender, SelectionChangedEventArgs e)
    {
        var sorted = _enters.ToList();

        switch (LoginComboBox.SelectedIndex)
        {
            case 0:
                sorted = _enters.OrderBy(s => s.UserLogin).ToList();
                break;
            case 1:
                sorted = _enters.OrderByDescending(s => s.UserLogin).ToList();
                break;
            case 2:
                sorted = _enters.ToList();
                break;
        }

        EnterBox.ItemsSource = sorted;
    }

    private void Sort_ComboBox_Date(object sender, SelectionChangedEventArgs e)
    {
        var sorted = _enters.ToList();

        switch (DateComboBox.SelectedIndex)
        {
            case 0:
                sorted = _enters.OrderBy(s => s.Time).ToList();
                break;
            case 1:
                sorted = _enters.OrderByDescending(s => s.Time).ToList();
                break;
            case 2:
                sorted = _enters.ToList();
                break;
        }

        EnterBox.ItemsSource = sorted;
    }
}