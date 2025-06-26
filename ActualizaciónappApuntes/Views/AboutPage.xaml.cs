using ActualizaciónappApuntes.ViewModels;

namespace ActualizaciónappApuntes.Views;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();
        BindingContext = new AboutViewModel();
    }
}