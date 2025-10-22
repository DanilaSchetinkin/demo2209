using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using demo2209.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

    }

    private void SessionTimer()
    {
        _sessionStartTime = DateTime.Now;
        _sessionTimeSeconds = TOTAL_SESSION_TIME_MINUTES * 60;

        _sessionTimer = new Timer(SessiomTimerCallback, null, 0, 1000);
    }

    private void SessionTimerCallback(object stater)
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
        WarningText.Visibility = Avalonia.Layout.Visibility.Visible;

        // Можно добавить звуковое оповещение или диалоговое окно
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
        // Останавливаем таймер
        _sessionTimer?.Dispose();

        // Сохраняем информацию о завершении сеанса
        SaveSessionEndInfo();

        // Показываем сообщение о завершении
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

        // Закрываем окно и блокируем систему
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
                LogoutTime = DateTime.Now,
                Employee = context.Employees.FirstOrDefault(e => e.Login == _fio), // Предполагая, что FIO это логин
                LoginType = "Автоматическое завершение по таймеру"
            };
            context.LoginHistories.Add(sessionEndRecord);
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            // Логируем ошибку, но не прерываем процесс
            System.Diagnostics.Debug.WriteLine($"Ошибка сохранения информации о сеансе: {ex.Message}");
        }
    }

    private void CloseWindowAndBlockSystem()
    {
        // Сохраняем время блокировки в настройках приложения
        var blockUntil = DateTime.Now.AddMinutes(BLOCK_TIME_MINUTES);
        // Здесь можно сохранить в базу данных, файл или настройки приложения
        SaveBlockTime(blockUntil);

        // Закрываем окно
        Close();
    }

    private void SaveBlockTime(DateTime blockUntil)
    {
        // Пример сохранения времени блокировки
        // В реальном приложении используйте базу данных или защищенное хранилище
        Properties.Settings.Default.BlockUntil = blockUntil;
        Properties.Settings.Default.Save();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        // Останавливаем таймер при закрытии окна
        _sessionTimer?.Dispose();
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

    private void Sort_ComboBox_Login(object sender, EventArgs e)
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

    private void Sort_ComboBox_Date(object sender, EventArgs e)
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