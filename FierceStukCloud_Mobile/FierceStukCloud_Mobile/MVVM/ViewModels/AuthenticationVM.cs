using FierceStukCloud_Mobile.MVVM.ViewModels.AbstractVM;
using FierceStukCloud_Mobile.Views;
using FierceStukCloud_NetStandardLib.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using Xamarin.Forms;

namespace FierceStukCloud_Mobile.ViewModels
{
    public class AuthenticationVM : BaseViewModel
    {
        #region Свойства и поля

        #region Поля хранения значеий
        private string _login;
        private string _secureString;

        private bool _isAuthentication;
        private string _serverAnswer;
        private string _serverStatus = "Соединение";
        #endregion

        public string SecurePassword { private get => _secureString; set => SetProperty(ref _secureString, value); }

        public string Login { get => _login; set => SetProperty(ref _login, value); }

        public bool IsAuthentication { get => _isAuthentication; set => SetProperty(ref _isAuthentication, value); }

        public string ServerAnswer { get => _serverAnswer; set => SetProperty(ref _serverAnswer, value); }

        public string ServerStatus { get => _serverStatus; set => SetProperty(ref _serverStatus, value); }

        #endregion

        public ICommand AuthenticationCommand { get; private set; }

        /// <summary>
        /// Команда авторизации
        /// </summary>
        /// <param name="parameter"></param>
        private void AutorizationMethod()
        {
            IsAuthentication = true;

            //FierceStukCloudSettings.Default.Login = Login;
            //FierceStukCloudSettings.Default.Password = SecurePassword;
            
            var client = new RestClient(App.CurSiteLing + "api/Authentication");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("username", "ForeverFast");
            request.AddHeader("password", "789xxx44XX");
            request.AddHeader("device", "Phone");
            IRestResponse response = client.Execute(request);

            int Code = 0;
            if (Int32.TryParse(response.Content, out Code) == true)
            {
                switch (Code)
                {
                    case 151:
                        ServerAnswer = "Неверный пароль";
                        break;
                    case 152:
                        ServerAnswer = "Такого логина не существует";
                        break;
                }
            }

            App.CurrentUser = JsonSerializer.Deserialize<User>(response.Content);

            if (App.CurrentUser != null)
            {
                IsAuthentication = false;
                ServerAnswer = "Авторизован";
                Navigation.PushAsync(new MainPage(new MusicPlayerVM() { Navigation = this.Navigation }));
            }
            else
            {
                IsAuthentication = false;
                ServerAnswer = "Ошибка входа";
            }
        }




        #region Конструкторы и методы инициализации

        public AuthenticationVM()
        {
            InitiailizeCommands();
           //Login = FierceStukCloudSettings.Default.Login != null ?
           //         FierceStukCloudSettings.Default.Login : "";

           // SecurePassword = FierceStukCloudSettings.Default.Password != null ?
           //                  FierceStukCloudSettings.Default.Password : "";
        }

        public override void InitiailizeCommands()
        {
            base.InitiailizeCommands();
            AuthenticationCommand = new Command(AutorizationMethod);
        }

        #endregion
    }
}
