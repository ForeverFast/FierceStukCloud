using FierceStukCloud_NetCoreLib.Models;
using FierceStukCloud_NetCoreLib.Services;
using FierceStukCloud_PC.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace FierceStukCloud_PC.MVVM.ViewModels
{
    class AutorizationVM : OnPropertyChangedClass
    {
        #region Управление окном

        public RelayCommand MinimizedWindowCommand { get; private set; }
        public RelayCommand CloseWindowCommand { get; private set; }
        public RelayCommand DragWindowCommand { get; private set; }

        public void MinimizedWindowMethod(object parameter) => Application.Current.MainWindow.WindowState = WindowState.Minimized;
        public void CloseWindowMethod(object parameter) => Application.Current.MainWindow.Close();
        public void DragWindowMethod(object parameter) => Application.Current.MainWindow.DragMove();

        #endregion



        #region Авторизация

        #region Свойства и поля

        #region Поля хранения значеий
        private string _login;
        private string _password;
       
        private string _defaultLoginTextShow;
        private string _defaultPasswordTextShow;
        private Visibility _loadGifVisiableAuthentication = Visibility.Hidden;   
        private Visibility _loadGifVisiableServerStatus = Visibility.Hidden;
      
        private bool _isAuthentication;
        private string _serverAnswer;
        private string _serverStatus = "Соединение";
        #endregion


        public string Login
        {
            get => _login;
            set
            {
                if (value == "")
                {
                    DefaultLoginTextShow = "Visible";
                    SetProperty(ref _login, value);
                }
                else
                {
                    DefaultLoginTextShow = "Hidden";
                    SetProperty(ref _login, value);
                }
                FSC_Settings.Default.Login = _login;
                FSC_Settings.Default.Save();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (value == "")
                {
                    DefaultPasswordTextShow = "Visible";
                    SetProperty(ref _password, value);
                }
                else
                {
                    DefaultPasswordTextShow = "Hidden";
                    SetProperty(ref _password, value);
                }
                FSC_Settings.Default.Password = _password;
                FSC_Settings.Default.Save();
            }
        }


        public string DefaultLoginTextShow { get => _defaultLoginTextShow; set => SetProperty(ref _defaultLoginTextShow, value); }

        public string DefaultPasswordTextShow { get => _defaultPasswordTextShow; set => SetProperty(ref _defaultPasswordTextShow, value); }

        public Visibility LoadGifVisiableAuthentication { get => _loadGifVisiableAuthentication; set => SetProperty(ref _loadGifVisiableAuthentication, value); }

        public Visibility LoadGifVisiableServerStatus { get => _loadGifVisiableServerStatus; set => SetProperty(ref _loadGifVisiableServerStatus, value); }

       
        public bool IsAuthentication
        {
            get => _isAuthentication;
            set
            {
                if (value == true)              
                    LoadGifVisiableAuthentication = Visibility.Visible;          
                else
                    LoadGifVisiableAuthentication = Visibility.Hidden;
            }
        }

        public string ServerAnswer
        {
            get => _serverAnswer;
            set => SetProperty(ref _serverAnswer, value);
        }

        public string ServerStatus
        {
            get => _serverStatus;
            set => SetProperty(ref _serverStatus, value);
        }

        #endregion


        public RelayCommand AutorizationCommand { get; private set; }

        private void AutorizationMethod(object parameter)
        {
            IsAuthentication = true;

            var client = new RestClient("http://fiercestukcloud.life/api/Authentication");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("username", Login);
            request.AddHeader("password", Password);
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
                OpenMainWindow();
            }
            else
            {
                IsAuthentication = false;
                ServerAnswer = "Ошибка входа";
            }
        }

        private async void OpenMainWindow()
        {
            var mainwindodVM = new MainWindowVM();

            App.DisplayRootRegistry.HidePresentation(this);
            await App.DisplayRootRegistry.ShowModalPresentation(mainwindodVM);
            App.DisplayRootRegistry.ClosePresentation(this);
        }

        #endregion



        #region Конструкторы

        public AutorizationVM()
        {
            InitiailizeCommands();

            #region Настройки окна авторизации
            Login = FSC_Settings.Default.Login;
            Password = FSC_Settings.Default.Password;
            
            if(Login != null && Password != null)
            {
                if(Login != "" && Password != "")
                {
                    this.AutorizationMethod(this);
                }
            }
            #endregion
        }

        private void InitiailizeCommands()
        {
            MinimizedWindowCommand = new RelayCommand(MinimizedWindowMethod, null);
            CloseWindowCommand = new RelayCommand(CloseWindowMethod, null);
            DragWindowCommand = new RelayCommand(DragWindowMethod, null);

            AutorizationCommand = new RelayCommand(AutorizationMethod, null);
        }

        #endregion
    }
}
