using Egor92.MvvmNavigation.Abstractions;
using FierceStukCloud.Core.Services;
using FierceStukCloud.Mvvm;

namespace FierceStukCloud.Pc.Mvvm.ViewModels.Abstractions
{
    public abstract class BaseViewModel : OnPropertyChangedClass
    {
        public INavigationManager _navigationManager;
        public IMusicPlayerService _musicPlayerService;
        public IDialogService _dialogService;
        public IMusicStorage _musicStorage;     
       
        #region Конструкторы и методы инициализации

        public abstract void InitiailizeCommands();

        #endregion
    }
}
