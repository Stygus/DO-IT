using DOIT.ViewModels; // Importowanie przestrzeni nazw z widokami
using Xceed.Maui.Toolkit; // Importowanie narz�dzi z Xceed
using Xceed.Maui.Toolkit.Themes.Brushes; // Importowanie narz�dzi do motyw�w i p�dzli

namespace DOIT; // Przestrze� nazw aplikacji

// Klasa reprezentuj�ca stron� rejestracji
public partial class RegisterPage : ContentPage
{
    private INavigation _navigation; // Zmienna do przechowywania nawigacji

    // Konstruktor klasy RegisterPage
    public RegisterPage(INavigation navigation)
    {
        InitializeComponent(); // Inicjalizacja komponent�w XAML
        this._navigation = navigation; // Przechowywanie referencji do obiektu nawigacji
        BindingContext = new RegisterViewModel(Navigation); // Ustawienie kontekstu danych dla strony
    }

    // Obs�uguje zdarzenie wci�ni�cia przycisku
    private void Button_PointerDown(object sender, EventArgs e)
    {
        // Sprawdzenie, czy sender to przycisk Xceed
        if (sender is Xceed.Maui.Toolkit.Button button)
        {
            // Zmiana koloru t�a przycisku na ��ty
            button.Background = Color.FromHex("#FFBF59");
        }
    }

    // Obs�uguje zdarzenie zwolnienia przycisku
    private void Button_PointerUp(object sender, EventArgs e)
    {
        // Rzutowanie sendera na przycisk Xceed
        Xceed.Maui.Toolkit.Button button = sender as Xceed.Maui.Toolkit.Button;
        // Przywr�cenie koloru t�a przycisku
        button.Background = Color.FromArgb("1a1e23");
    }

    // Obs�uguje zdarzenie klikni�cia na etykiet� (np. do przej�cia do ekranu logowania)
    public async void OnLabelTapped(object sender, EventArgs e)
    {
        // Powr�t do poprzedniej strony w stosie nawigacji
        await this._navigation.PopAsync();
    }
}
