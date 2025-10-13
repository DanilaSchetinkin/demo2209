using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace demo2209;

public partial class PeopleWindow : Window
{
    private string _fio;
    private string _role;
    private string _imagePath;


    public PeopleWindow()
    {
        InitializeComponent();
    }

    public PeopleWindow(string fio, string role, string imagePath)
    {
        InitializeComponent();
    }
}