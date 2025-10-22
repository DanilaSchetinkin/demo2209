using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace demo2209;

public partial class OrderWindow : Window
{
    private string _fio;
    private string _role;
    private Bitmap? _image;

    public OrderWindow()
    {
        InitializeComponent();
    }

    public OrderWindow(string fio, string role, Bitmap? image)
    {
        InitializeComponent();

        _fio = fio;
        _role = role;
        _image = image;

        imageBox.Source = _image;
        fioBox.Text = _fio;
        roleBox.Text = _role;
    }
}