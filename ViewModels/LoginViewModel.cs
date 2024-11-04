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

        public string webApiKey = "AIzaSyA_I74E8Vc9HXg6P5b2Qe4bNzRYCll8fuk";
        private INavigation _navigation;
        private string userName;
        private string userPassword;

        public event PropertyChangedEventHandler PropertyChanged;

        public Command RegisterBtn { get; }
        public Command LoginBtn { get; }
        public string UserName
        {
            get => userName; set
            {
                userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        public string UserPassword
        {
            get => userPassword; set
            {
                userPassword = value;
                RaisePropertyChanged("UserPassword");
            }
        }

        public LoginViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            RegisterBtn = new Command(RegisterBtnTappedAsync);
            LoginBtn = new Command(LoginBtnTappedAsync);
        }

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
                if (ex.Reason == AuthErrorReason.InvalidEmailAddress) {
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

        private void RegisterBtnTappedAsync(object obj)
        {
            this._navigation.PushAsync(new RegisterPage(_navigation));
        }
        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }

}
