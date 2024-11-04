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

        public string webApiKey = "AIzaSyA_I74E8Vc9HXg6P5b2Qe4bNzRYCll8fuk";

        private INavigation _navigation;
        private string email = "";
        private string password = "";
        private string repassword = "";
        private string nick = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }
       

        public string Password
        {
            get => password; set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }
        public string RePassword
        {
            get => repassword; set
            {
                repassword = value;
                RaisePropertyChanged("RePassword");
            }
        }
        public string Nick
        {
            get => nick; set
            {
                nick = value;
                RaisePropertyChanged("Nick");
            }
        }



        public Command RegisterUser { get; }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public RegisterViewModel(INavigation navigation)
        {
            this._navigation = navigation;

            RegisterUser = new Command(RegisterUserTappedAsync);
        }

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
                        await App.Current.MainPage.DisplayAlert("Alert", "User Registered successfully", "OK");
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
