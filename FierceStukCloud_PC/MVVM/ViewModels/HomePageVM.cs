using Egor92.MvvmNavigation.Abstractions;
using FierceStukCloud_NetCoreLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FierceStukCloud_PC.MVVM.ViewModels
{
    public class HomePageVM : BaseViewModel, INavigatedToAware
    {
        private readonly INavigationManager _navigationManager;

        public HomePageVM(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void OnNavigatedTo(object arg)
        {
            throw new NotImplementedException();
        }
    }
}
