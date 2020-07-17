using FierceStukCloud_Mobile.Models;
using FierceStukCloud_NetStandardLib.Models;
using FierceStukCloud_NetStandardLib.Models.AbstractModels;
using FierceStukCloud_NetStandardLib.Models.MusicContainers;
using FierceStukCloud_NetStandardLib.MVVM;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static FierceStukCloud_NetStandardLib.Types.CustomEnums;

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

        public async Task MusicPlayerCommand(Commands command, DeviceType device)
            => await hubConnection.SendAsync("MusicPlayerCommand", DeviceType.Mobile, device, command);

        public async Task SetCurrentSongCommand(DeviceType device, Song song)
           => await hubConnection.SendAsync("SetCurrentSongCommand", DeviceType.Mobile, device, song);

        public async Task CommandFromPС(DeviceType deviceFrom, string json1, string json2)
        {
            var temp1 = JsonSerializer.Deserialize<List<LocalFolder>>(json1);
            var temp2 = JsonSerializer.Deserialize<List<Song>>(json2);
            foreach (var item in temp1)
            {
                Model.LocalFiles.Add(item);
            }

            foreach (var item in temp2)
            {
                Model.LocalFiles.Add(item);
            }

            Update?.Invoke(this);
        }

        public event Action<object> Update;





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
                    options.AccessTokenProvider = () => Task.FromResult(App.CurrentUser.AccessTokenPhone);
                })
                .Build();
             
            hubConnection.On<DeviceType, string, string>("SendSongs", CommandFromPС);
            hubConnection.On("TestPC", () => 
            {
                int a = 1 + 1;
            });

            hubConnection.ServerTimeout = new TimeSpan(0, 10, 0);

            Connect();

            //hubConnection.SendAsync("TestPhone");
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
