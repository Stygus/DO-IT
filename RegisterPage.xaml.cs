using DOIT.ViewModels;
using Xceed.Maui.Toolkit;
using Xceed.Maui.Toolkit.Themes.Brushes;

namespace DOIT;

public partial class RegisterPage : ContentPage
{
    private INavigation _navigation;
    public RegisterPage(INavigation navigation)
    {
        InitializeComponent();
        this._navigation = navigation;
        BindingContext = new RegisterViewModel(Navigation);
    }



    private void Button_PointerDown(object sender, EventArgs e)
    {
        if (sender is Xceed.Maui.Toolkit.Button button)
        {
            button.Background = Color.FromHex("#FFBF59");
        }

    }

    private void Button_PointerUp(object sender, EventArgs e)
    {
        Xceed.Maui.Toolkit.Button button = sender as Xceed.Maui.Toolkit.Button;
        button.Background = Color.FromArgb("1a1e23");
    }

    public async void OnLabelTapped(object sender, EventArgs e)
    {
        await this._navigation.PopAsync();
    }
}