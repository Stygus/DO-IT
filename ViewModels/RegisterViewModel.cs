using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed;
using Xceed.Maui.Toolkit;

namespace DOIT.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    internal class RegisterViewModel : INotifyPropertyChanged
    {
        /// Klucz API do uwierzytelniania w Firebase.
        public string webApiKey = "AIzaSyA_I74E8Vc9HXg6P5b2Qe4bNzRYCll8fuk";

        private INavigation _navigation;
        private string email = "";
        private string password = "";
        private string repassword = "";
        private string nick = "";

        /// Wydarzenie zgłaszane, gdy właściwość zostaje zmieniona.
        public event PropertyChangedEventHandler PropertyChanged;

        /// Adres e-mail użytkownika.
        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        /// Hasło użytkownika.
        public string Password
        {
            get => password;
            set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }

        /// Powtórzone hasło użytkownika.
        public string RePassword
        {
            get => repassword;
            set
            {
                repassword = value;
                RaisePropertyChanged("RePassword");
            }
        }

        /// Pseudonim (nick) użytkownika.
        public string Nick
        {
            get => nick;
            set
            {
                nick = value;
                RaisePropertyChanged("Nick");
            }
        }

        /// Polecenie obsługujące rejestrację użytkownika.
        public Command RegisterUser { get; }

        /// Polecenie obsługujące powrót do ekranu logowania.
        public Command Login { get; }

        /// Zgłasza zdarzenie zmiany właściwości.
        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        /// Konstruktor, inicjalizujący polecenia i przypisujący nawigację.
        public RegisterViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            Login = new Command(UserTappedLoginAsync);
            RegisterUser = new Command(RegisterUserTappedAsync);
        }

        /// Obsługuje powrót do poprzedniego ekranu po kliknięciu przycisku logowania.
        private async void UserTappedLoginAsync()
        {
            await this._navigation.PopAsync();
        }

        /// Obsługuje rejestrację użytkownika po kliknięciu przycisku rejestracji.
        private async void RegisterUserTappedAsync(object obj)
        {
            if (password.Length <= 6 || password == null)
            {
                App.Current.MainPage.DisplayAlert("", "Za krótkie hasło", "OK");
                return;
            }
            if (password != repassword)
            {
                App.Current.MainPage.DisplayAlert("", "Podane hasła nie są identyczne", "OK");
                return;
            }
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);
                string token = auth.FirebaseToken;
                if (token != null)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "Rejestracja przebiegła pomyślnie", "OK");
                    await authProvider.SendEmailVerificationAsync(auth.FirebaseToken);
                }
                await this._navigation.PopAsync();
            }
            catch (FirebaseAuthException ex)
            {
                if (ex.Reason == AuthErrorReason.InvalidEmailAddress)
                {
                    await App.Current!.MainPage!.DisplayAlert("Alert", "Adres E-mail jest nieprawidłowy", "OK");
                    return;
                }

                if (ex.Reason == AuthErrorReason.EmailExists)
                {
                    await App.Current!.MainPage!.DisplayAlert("Alert", "Adres E-mail jest już zajęty", "OK");
                    return;
                }

                if (ex.Reason == AuthErrorReason.WeakPassword)
                {
                    await App.Current!.MainPage!.DisplayAlert("Alert", "Podane hasło jest za słabe", "OK");
                    return;
                }

                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
        }
    }
}
