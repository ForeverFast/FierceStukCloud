using FierceStukCloud_Mobile.Models;
using FierceStukCloud_NetStandardLib.Models;
using FierceStukCloud_NetStandardLib.Models.AbstractModels;
using FierceStukCloud_NetStandardLib.Models.MusicContainers;
using FierceStukCloud_NetStandardLib.MVVM;
using Microsoft.AspNetCore.SignalR.Client;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static FierceStukCloud_NetStandardLib.Types.CustomEnums;

namespace FierceStukCloud_Mobile.MVVM.Models.Modules
{
    public class MWM_SignalR
    {
        private HubConnection hubConnection;

        #region Свойства

        /// <summary> Идет ли отправка сообщений </summary>
        public bool IsBusy { get; set; }

        /// <summary> Осуществлено ли подключение </summary>
        public bool IsConnected { get; set; }

        #endregion

        #region Отправка сообщений

        public async Task MusicPlayerCommand(Commands command, DeviceType device)
            => await hubConnection.SendAsync("MusicPlayerCommand", DeviceType.Mobile, device, command);

        public async Task SetCurrentSongCommand(DeviceType device, Song song)
           => await hubConnection.SendAsync("SetCurrentSongCommand", DeviceType.Mobile, device, song);

        #endregion

        #region Получение сообщений

        /// <summary>
        /// Получение списка локальных файлов
        /// </summary>
        /// <param name="deviceFrom"></param>
        /// <param name="json1"></param>
        /// <param name="json2"></param>
        public void CommandFromPС(DeviceType deviceFrom, string json1, string json2)
        {
            try
            {
                List<BaseMusicObject> temp = new List<BaseMusicObject>();

                json1 = Regex.Replace(json1, @"\\u([0-9A-Fa-f]{4})", m => "" + (char)Convert.ToInt32(m.Groups[1].Value, 16));
                json2 = Regex.Replace(json2, @"\\u([0-9A-Fa-f]{4})", m => "" + (char)Convert.ToInt32(m.Groups[1].Value, 16));

                foreach (var item in JsonSerializer.Deserialize<List<LocalFolder>>(json1))
                    temp.Add(item);

                foreach (var item in JsonSerializer.Deserialize<List<Song>>(json2))
                    temp.Add(item);

                UpdateInfoFromPC?.Invoke(temp);
            }
            catch(Exception ex)
            {

            }
        }

        public void NewCurrentSong(DeviceType deviceFrom, Song song)
            => _NewCurrentSong?.Invoke(song);

        #endregion

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

        #region События

        public event Action<List<BaseMusicObject>> UpdateInfoFromPC;
        public event Action<Song> _NewCurrentSong;

        #endregion

        #region Конструкторы 

        public MWM_SignalR()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(App.CurSiteLing + "hub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(App.CurrentUser.AccessTokenPhone);
                })
                .Build();

            hubConnection.ServerTimeout = new TimeSpan(0, 10, 0);
            hubConnection.On<DeviceType, string, string>("SendSongs", CommandFromPС);
            hubConnection.On<DeviceType, Song>("NewCurrentSong", NewCurrentSong);

            hubConnection.Closed += async (error) =>
            {
                IsConnected = false;
                await Connect();
            };

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
