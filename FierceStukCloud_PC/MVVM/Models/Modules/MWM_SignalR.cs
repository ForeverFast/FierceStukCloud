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
        private HubConnection hubConnection { get; set; }
        private MainWindowM Model { get; set; }

        #region Свойства

        #region Поля 
        private bool _isBusy;
        private bool _isConnected;
        #endregion


        /// <summary> Идет ли отправка сообщений </summary>
        public bool IsBusy { get => _isBusy; set { if (_isBusy != value) SetProperty(ref _isBusy, value); } }

        /// <summary> Осуществлено ли подключение </summary>
        public bool IsConnected { get => _isConnected; set { if (_isConnected != value) SetProperty(ref _isConnected, value); } }

        #endregion

        private async Task CommandFromPhone(DeviceType deviceFrom, Commands command)
        {
            try
            {


                switch (command)
                {
                    case Commands.GetSongs:

                        List<LocalFolder> LocalFolders;
                        List<Song> Songs;

                        Model.LocalFolders.TrySort(out LocalFolders, out Songs);

                        var json1 = JsonSerializer.Serialize<List<LocalFolder>>(LocalFolders);
                        var json2 = JsonSerializer.Serialize<List<Song>>(Songs);

                        await hubConnection.SendAsync("SendSongsCommand", DeviceType.PC, deviceFrom, json1, json2);

                        break;

                    case Commands.PrevSong:

                        Model.PrevSong();

                        break;
                    case Commands.NextSong:

                        Model.NextSong();

                        break;
                    case Commands.PlaySong:

                        Model.Play();

                        break;
                    case Commands.PauseSong:

                        Model.Pause();

                        break;
                    case Commands.StopSong:

                        Model.Stop();

                        break;
                }
            }
            catch(Exception ex)
            {

            }
        }

        private async Task SetCurrentSong(DeviceType deviceFrom, Song song)
        {
            //foreach (var item in Model.LocalFiles)
            //{
            //    if(item is LocalFolder)
            //    {
            //        var temp = item.ToMC().Songs.Find(x=> x.)
            //    }
            //}
            //var p = .Find(x => x.Title == song.)
            Model.SetCurrentSong(song);

        }

        #region Основные методы

        /// <summary>
        /// Подключение к серверу
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Отключение от сервера
        /// </summary>
        /// <returns></returns>
        public async Task Disconnect()
        {
            if (!IsConnected)
                return;

            await hubConnection.StopAsync();
            IsConnected = false;

        }

        #endregion


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

            hubConnection.Closed += async (Exception) =>
            {
                IsConnected = false;
                //DialogService.ShowMessage(Exception);
                await Connect();
            };

            hubConnection.ServerTimeout = new TimeSpan(0, 10, 0);
           
            Task.Run(() => Connect());
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
