using FierceStukCloud_Mobile.Models;
using FierceStukCloud_NetStandardLib.MVVM;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static FierceStukCloud_Mobile.Types.CustomEnums;

namespace FierceStukCloud_Mobile.MVVM.Models.Modules
{
    public class MWM_SignalR : OnPropertyChangedClass
    {
        private HubConnection hubConnection;
        private MusicPlayerM Model;

        #region Свойства

        #region Поля 
        private bool _isBusy;
        private bool _isConnected;
        #endregion

        #endregion

        /// <summary> Идет ли отправка сообщений </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    // OnPropertyChanged("IsBusy");
                }
            }
        }

        /// <summary> Осуществлено ли подключение </summary>
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    //  OnPropertyChanged("IsConnected");
                }
            }
        }


        public async Task CommandFromPС(string command, object[] objects)
        {
            Commands _command;
            if (command.TryConvert(out _command) == true)
                switch (_command)
                {
                    case Commands.SendSongs:

                        

                        break;

                    case Commands.SetCurrentSong:

                        //Model.SetCurrentSong(objects[0] as Song);

                        break;
                }

        }

        public async Task CommandToPC<T>(Commands command, T content)
        {
            switch (command)
            {
                case Commands.GetSongs:

                    //

                    break;

                case Commands.SetCurrentSong:

                    await hubConnection.SendAsync("Send", Commands.SendSongs.ToString(), Model.LocalFiles);
                   
                    break;
            }
        }

        // подключение к чату
        public async Task Connect()
        {
            if (IsConnected)
                return;
            try
            {
                await hubConnection.StartAsync();
                

                IsConnected = true;
            }
            catch (Exception)
            {
                
            }
        }

        // Отключение от чата
        public async Task Disconnect()
        {
            if (!IsConnected)
                return;

            await hubConnection.StopAsync();
            IsConnected = false;
           
        }


        #region Конструкторы 

        public MWM_SignalR(MusicPlayerM Model)
        {
            this.Model = Model;

            hubConnection = new HubConnectionBuilder()
                .WithUrl(App.CurSiteLing + "hub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(App.CurrentUser.AccessTokenPC);
                })
                .Build();

            hubConnection.On<string, object[]>("MessageFromPС", CommandFromPС);

            Connect();
        }

        #endregion
    }
}

/* Не используется
 
 hubConnection.Closed += async (error) =>
            {
                //SendLocalMessage(String.Empty, "Подключение закрыто...");
                IsConnected = false;
                await Task.Delay(5000);
                await Connect();
            };

 */
