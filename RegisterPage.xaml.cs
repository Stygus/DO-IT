using DOIT.ViewModels; // Importowanie przestrzeni nazw z widokami
using Xceed.Maui.Toolkit; // Importowanie narzêdzi z Xceed
using Xceed.Maui.Toolkit.Themes.Brushes; // Importowanie narzêdzi do motywów i pêdzli

namespace DOIT; // Przestrzeñ nazw aplikacji

// Klasa reprezentuj¹ca stronê rejestracji
public partial class RegisterPage : ContentPage
{
    private INavigation _navigation; // Zmienna do przechowywania nawigacji

    // Konstruktor klasy RegisterPage
    public RegisterPage(INavigation navigation)
    {
        InitializeComponent(); // Inicjalizacja komponentów XAML
        this._navigation = navigation; // Przechowywanie referencji do obiektu nawigacji
        BindingContext = new RegisterViewModel(Navigation); // Ustawienie kontekstu danych dla strony
    }

    // Obs³uguje zdarzenie wciœniêcia przycisku
    private void Button_PointerDown(object sender, EventArgs e)
    {
        // Sprawdzenie, czy sender to przycisk Xceed
        if (sender is Xceed.Maui.Toolkit.Button button)
        {
            // Zmiana koloru t³a przycisku na ¿ó³ty
            button.Background = Color.FromHex("#FFBF59");
        }
    }

    // Obs³uguje zdarzenie zwolnienia przycisku
    private void Button_PointerUp(object sender, EventArgs e)
    {
        // Rzutowanie sendera na przycisk Xceed
        Xceed.Maui.Toolkit.Button button = sender as Xceed.Maui.Toolkit.Button;
        // Przywrócenie koloru t³a przycisku
        button.Background = Color.FromArgb("1a1e23");
    }

    // Obs³uguje zdarzenie klikniêcia na etykietê (np. do przejœcia do ekranu logowania)
    public async void OnLabelTapped(object sender, EventArgs e)
    {
        // Powrót do poprzedniej strony w stosie nawigacji
        await this._navigation.PopAsync();
    }
}
