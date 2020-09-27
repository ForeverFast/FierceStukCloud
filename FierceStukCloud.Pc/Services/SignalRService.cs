using FierceStukCloud.Abstractions;
using FierceStukCloud.Core;
using FierceStukCloud.Core.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using static FierceStukCloud.Core.CustomEnums;

namespace FierceStukCloud.Pc.Services
{
    public class SignalRService : OnPropertyChangedClass, ISignalRService
    {
        #region Свойства

        #region Поля 
        private bool _isBusy;
        private bool _isConnected;
        #endregion

        public HubConnection HubConnection { get; set; }

        /// <summary> Идет ли отправка сообщений </summary>
        public bool IsBusy { get => _isBusy; set { if (_isBusy != value) SetProperty(ref _isBusy, value); } }

        /// <summary> Осуществлено ли подключение </summary>
        public bool IsConnected { get => _isConnected; set { if (_isConnected != value) SetProperty(ref _isConnected, value); } }

        #endregion

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
            //Model.SetCurrentSong(song);

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
                await HubConnection.StartAsync();
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

            await HubConnection.StopAsync();
            IsConnected = false;

        }

        public async Task IncomingCommands(DeviceType deviceFrom, Commands command)
        {
            try
            {
                OnPropertyChanged(command.ToString());

                //switch (command)
                //{
                //    case Commands.GetSongs:

                //        List<LocalFolder> LocalFolders;
                //        List<Song> Songs;

                //        Model.LocalFolders.TrySort(out LocalFolders, out Songs);

                //        var json1 = JsonSerializer.Serialize<List<LocalFolder>>(LocalFolders);
                //        var json2 = JsonSerializer.Serialize<List<Song>>(Songs);

                //        await HubConnection.SendAsync("SendSongsCommand", DeviceType.PC, deviceFrom, json1, json2);

                //        break;

                //    case Commands.PrevSong:

                //        OnPropertyChanged("PrevSong");

                //        break;
                //    case Commands.NextSong:

                //        OnPropertyChanged("NextSong");

                //        break;
                //    case Commands.PlaySong:

                //        OnPropertyChanged("PlaySong");

                //        break;
                //    case Commands.PauseSong:

                //        OnPropertyChanged("PauseSong");

                //        break;
                //    case Commands.StopSong:

                //        OnPropertyChanged("StopSong");

                //        break;
                //}
            }
            catch (Exception ex)
            {

            }
        }



        #endregion


        #region Конструкторы 

        public SignalRService()
        {
            //HubConnection = new HubConnectionBuilder()
            //    .WithUrl(App.CurSiteLink + "hub", options =>
            //    {
            //        options.AccessTokenProvider = () => Task.FromResult(App.CurrentUser.AccessTokenPC);
            //    })
            //    .Build();

            //HubConnection.On<DeviceType, Commands>("Commands", IncomingCommands);
            //HubConnection.On<DeviceType, Song>("SetCurrentSong", SetCurrentSong);

            //HubConnection.Closed += async (Exception) =>
            //{
            //    IsConnected = false;
            //    //DialogService.ShowMessage(Exception);
            //    await Connect();
            //};

            //HubConnection.ServerTimeout = new TimeSpan(0, 10, 0);

            //Task.Run(() => Connect());
        }

        #endregion
    }
}
