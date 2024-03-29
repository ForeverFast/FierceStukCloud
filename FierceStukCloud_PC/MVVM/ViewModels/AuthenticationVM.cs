﻿using System;
using System.Text.Json;
using RestSharp;
using FierceStukCloud_NetCoreLib.ViewModels;
using System.Windows.Threading;
using static System.Diagnostics.Debug;
using FierceStukCloud.Pс;
using FierceStukCloud.Core;

namespace FierceStukCloud_PC.MVVM.ViewModels
{
    class AutorizationVM : BaseViewModel
    {
        public Dispatcher Dispatcher { get; }


        #region Авторизация

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


        #region Команды  
        public RelayCommand AutorizationCommand { get; private set; }

        /// <summary>
        /// Команда авторизации
        /// </summary>
        /// <param name="parameter"></param>
        private void AutorizationMethod(object parameter)
        {
            IsAuthentication = true;
            //App.Log.Info("Авторизация...");


            //OpenMainWindow();
            FSC_Settings.Default.Login = Login;
            FSC_Settings.Default.Password = SecurePassword;
            FSC_Settings.Default.Save();

           
            var client = new RestClient(App.CurSiteLing + "api/Authentication");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("username", Login);
            request.AddHeader("password", "789xxx44XX");
            request.AddHeader("device", "PC");
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
                //App.Log.Info("Авторизирован.");
                OpenMainWindow();
            }
            else
            {
                IsAuthentication = false;
                ServerAnswer = "Ошибка входа";
                //App.Log.Info("Ошибка входа.");
            }
        }

        private void OpenMainWindow()
        {
            //var mainwindodVM = new MainWindowVM(Dispatcher);

            //App.DisplayRootRegistry.HidePresentation(this);
            //await App.DisplayRootRegistry.ShowModalPresentation(mainwindodVM);
            //App.DisplayRootRegistry.ClosePresentation(this);
        }

        #endregion

        #endregion


        #region Конструкторы и доп. методы

        public AutorizationVM()
        {
            WriteLine("Создание VM для окна авторизации.");
            InitiailizeCommands();
            
            
            Login = FSC_Settings.Default.Login;

            //SecurePassword = new NetworkCredential("",FSC_Settings.Default.Password).SecurePassword;

            //if (Login != null && SecurePassword != null)
            //{
            //    if (Login != "" && SecurePassword.Length != 0)
            //    {
            //        this.AutorizationMethod(this);
            //    }
            //}

            WriteLine("Создание VM для окна авторизации завершено.");
           //App.Log.Trace("Тест");
            
        }

        public AutorizationVM(Dispatcher dispatcher)
            : this()
        {
            Dispatcher = dispatcher;
        }

        /// <summary>
        /// Инициализация команд
        /// </summary>
        public override void InitiailizeCommands()
        {
            WriteLine("Инициализация команд VM для окна авторизации.");
            base.InitiailizeCommands();
            AutorizationCommand = new RelayCommand(AutorizationMethod, null);
            WriteLine("Инициализация команд VM для окна авторизации выполнена успешно.");
        }

        #endregion
    }
}
