using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOIT.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    internal class LoginViewModel
    {
        /// Klucz API dla uwierzytelniania w Firebase.
        public string webApiKey = "AIzaSyA_I74E8Vc9HXg6P5b2Qe4bNzRYCll8fuk";

        private INavigation _navigation;
        private string userName;
        private string userPassword;

        /// Wydarzenie zgłaszane, gdy właściwość zostaje zmieniona.
        public event PropertyChangedEventHandler PropertyChanged;

        /// Polecenie obsługujące przycisk rejestracji.
        public Command RegisterBtn { get; }

        /// Polecenie obsługujące przycisk logowania.
        public Command LoginBtn { get; }

        /// Nazwa użytkownika.
        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        /// Hasło użytkownika.
        public string UserPassword
        {
            get => userPassword;
            set
            {
                userPassword = value;
                RaisePropertyChanged("UserPassword");
            }
        }

        /// Konstruktor ViewModelu logowania.
        /// <param name="navigation">Interfejs do nawigacji między stronami.</param>
        public LoginViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            RegisterBtn = new Command(RegisterBtnTappedAsync);
            LoginBtn = new Command(LoginBtnTappedAsync);
        }

        /// Metoda obsługująca logowanie po kliknięciu przycisku.
        /// <param name="obj">Parametr polecenia.</param>
        private async void LoginBtnTappedAsync(object obj)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(UserName, UserPassword);
                var content = await auth.GetFreshAuthAsync();
                var serializedContent = JsonConvert.SerializeObject(content);
                Preferences.Set("FreshFirebaseToken", serializedContent);
                await this._navigation.PushAsync(new Dashboard());
            }
            catch (FirebaseAuthException ex)
            {
                if (ex.Reason == AuthErrorReason.InvalidEmailAddress)
                {
                    await App.Current!.MainPage!.DisplayAlert("", "Nieprawidłowy E-mail", "OK");
                    return;
                }

                if (ex.Reason == AuthErrorReason.WrongPassword)
                {
                    await App.Current!.MainPage!.DisplayAlert("", "Nieprawidłowe hasło", "OK");
                    return;
                }

                if (ex.Reason == AuthErrorReason.Undefined)
                {
                    await App.Current!.MainPage!.DisplayAlert("", "Nieprawidłowe dane logowania", "OK");
                    return;
                }

                await App.Current.MainPage.DisplayAlert("", ex.Message, "OK");
            }
        }

        /// Metoda obsługująca rejestrację po kliknięciu przycisku.
        /// <param name="obj">Parametr polecenia.</param>
        private void RegisterBtnTappedAsync(object obj)
        {
            this._navigation.PushAsync(new RegisterPage(_navigation));
        }

        /// Metoda pomocnicza do zgłaszania zmiany właściwości.
        /// <param name="v">Nazwa zmienionej właściwości.</param>
        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}
