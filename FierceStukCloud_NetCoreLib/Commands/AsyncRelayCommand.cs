using System;
using System.Threading.Tasks;

namespace FierceStukCloud_NetCoreLib.Commands
{
    public class AsyncRelayCommand : AsyncCommandBase
    {
        private readonly Func<object, Task> _callback;

        public AsyncRelayCommand(Func<object, Task> callback, Action<Exception> onException) : base(onException)
        {
            _callback = callback;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            await _callback(parameter);
        }
    }
}
