using System;
using System.Windows;
using System.Security;
using System.Text.Json;
using System.Net;
using RestSharp;
using FierceStukCloud_NetCoreLib.Models;
using FierceStukCloud_NetCoreLib.Services;
using System.Windows.Threading;

namespace FierceStukCloud_PC.MVVM.ViewModels
{
    class AutorizationVM : OnPropertyChangedClass
    {

        public Dispatcher Dispatcher { get; }

        public AutorizationVM(Dispatcher dispatcher)
            : this()
        {
            Dispatcher = dispatcher;
        }


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
        private SecureString _secureString;

        private Visibility _defaultLoginTextShow;
        private Visibility _defaultPasswordTextShow;
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
                if (value != null)
                {
                    if (value == "")
                    {
                        DefaultLoginTextShow = Visibility.Visible;
                        SetProperty(ref _login, value);
                    }
                    else
                    {
                        DefaultLoginTextShow = Visibility.Hidden;
                        SetProperty(ref _login, value);
                    }
                    FSC_Settings.Default.Login = _login;
                    FSC_Settings.Default.Save();
                }
            }
        }

        public SecureString SecurePassword
        {
            private get => _secureString;
            set
            {

                if (value != null)
                {
                    if (value.Length == 0)
                    {
                        DefaultPasswordTextShow = Visibility.Visible;
                        _secureString = value;
                    }
                    else
                    {
                        DefaultPasswordTextShow = Visibility.Hidden;
                        _secureString = value;
                    }
                    FSC_Settings.Default.Password = value.ToString();
                    FSC_Settings.Default.Save();
                }
            }
        }


        public Visibility DefaultLoginTextShow { get => _defaultLoginTextShow; set => SetProperty(ref _defaultLoginTextShow, value); }

        public Visibility DefaultPasswordTextShow { get => _defaultPasswordTextShow; set => SetProperty(ref _defaultPasswordTextShow, value); }

        public Visibility LoadGifVisiableAuthentication { get => _loadGifVisiableAuthentication; set => SetProperty(ref _loadGifVisiableAuthentication, value); }

        public Visibility LoadGifVisiableServerStatus { get => _loadGifVisiableServerStatus; set => SetProperty(ref _loadGifVisiableServerStatus, value); }

       
        public bool IsAuthentication
        {
            get => _isAuthentication;
            set
            {
                SetProperty(ref _isAuthentication, value);
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

        #region Команды  
        public RelayCommand AutorizationCommand { get; private set; }

        /// <summary>
        /// Команда авторизации
        /// </summary>
        /// <param name="parameter"></param>
        private void AutorizationMethod(object parameter)
        {
            IsAuthentication = true;

#if DEBUG
            OpenMainWindow();
            return;
#endif

            var client = new RestClient("http://fiercestukcloud.life/api/Authentication");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("username", Login);
            request.AddHeader("password", SecurePassword.ToString());

            var q = SecurePassword.ToString();
            SecurePassword.Dispose();
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
            var mainwindodVM = new MainWindowVM(Dispatcher);

            App.DisplayRootRegistry.HidePresentation(this);
            await App.DisplayRootRegistry.ShowModalPresentation(mainwindodVM);
            App.DisplayRootRegistry.ClosePresentation(this);
        }

        #endregion

        #endregion


        #region Конструкторы и доп. методы

        public AutorizationVM()
        {
            InitiailizeCommands();

            #region Настройки окна авторизации
            Login = FSC_Settings.Default.Login;
            SecurePassword = new NetworkCredential("",FSC_Settings.Default.Password).SecurePassword;

            //if (Login != null && SecurePassword != null)
            //{
            //    if (Login != "" && SecurePassword.Length != 0)
            //    {
            //        this.AutorizationMethod(this);
            //    }
            //}
            #endregion
        }

        /// <summary>
        /// Инициализация команд
        /// </summary>
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
