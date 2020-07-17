using FierceStukCloud_NetStandardLib.Models;
using FierceStukCloud_NetCoreLib.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FierceStukCloud_NetStandardLib.MVVM;
using static FierceStukCloud_NetStandardLib.Types.CustomEnums;
using static FierceStukCloud_NetStandardLib.Extension.TypeConventer;
using System.Text.Json;
using FierceStukCloud_NetStandardLib.Models.AbstractModels;
using FierceStukCloud_NetStandardLib.Models.MusicContainers;
using FierceStukCloud_NetStandardLib.Extension;

namespace FierceStukCloud_PC.MVVM.Models.Modules
{
    public class MWM_SignalR : OnPropertyChangedClass
    {
        private HubConnection hubConnection;
        private MainWindowM Model;

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


        public async Task CommandFromPhone(DeviceType deviceFrom, Commands command)
        {

            switch (command)
            {
                case Commands.GetSongs:

                    List<LocalFolder> LocalFolders;
                    List<Song> Songs;

                    Model.LocalFiles.TrySort(out LocalFolders, out Songs);

                    var json1 = JsonSerializer.Serialize<List<LocalFolder>>(LocalFolders);
                    var json2 = JsonSerializer.Serialize<List<Song>>(Songs);

                    await hubConnection.SendAsync("SendSongsCommand", DeviceType.PC, deviceFrom, json1, json2);

                    break;

                case Commands.PrevSong:

                   

                    break;
                case Commands.NextSong:

                    

                    break;
                case Commands.PlaySong:

                   

                    break;
                case Commands.PauseSong:

                    

                    break;
                case Commands.StopSong:

                    

                    break;
            }

        }

        public async Task SetCurrentSong(DeviceType deviceFrom, Song song)
        {
            Model.SetCurrentSong(song);
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
            catch (Exception ex)
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

        public MWM_SignalR(MainWindowM Model)
        {
            this.Model = Model;

            hubConnection = new HubConnectionBuilder()
                .WithUrl(App.CurSiteLing + "hub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(App.CurrentUser.AccessTokenPC);
                })
                .Build();

            hubConnection.On<DeviceType, Commands>("Commands", CommandFromPhone);
            hubConnection.On<DeviceType, Song>("SetCurrentSong", SetCurrentSong);
            //hubConnection.On("TestPhone", () =>
            //{
            //    int a = 1 + 1;
            //});

            hubConnection.Closed += async (Exception) =>
            {
                IsConnected = false;
                //DialogService.ShowMessage(Exception);
                Connect();
            };

            hubConnection.ServerTimeout = new TimeSpan(0, 10, 0);
            

            


            //hubConnection.On<DeviceType, Commands>("Commands", CommandFromPhone);

            Connect();


            //hubConnection.SendAsync("TestPC");
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
