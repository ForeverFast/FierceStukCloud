using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using static FierceStukCloud.Core.CustomEnums;

namespace FierceStukCloud.Core.Services
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
