using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static FierceStukCloud_NetStandardLib.Types.CustomEnums;

namespace FierceStukCloud_NetStandardLib.Services
{
    public interface ISignalRService
    {
        HubConnection HubConnection { get; set; }

        /// <summary>
        /// Подключение к серверу
        /// </summary>
        /// <returns></returns>
        Task Connect();

        /// <summary>
        /// Отключение от сервера
        /// </summary>
        /// <returns></returns>
        Task Disconnect();

        /// <summary>
        /// Входящие команды без доп. параметра
        /// </summary>
        /// <param name="deviceFrom"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        Task IncomingCommands(DeviceType deviceFrom, Commands command);
    }
}
