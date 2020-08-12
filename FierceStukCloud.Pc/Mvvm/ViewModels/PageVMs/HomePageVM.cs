using Egor92.MvvmNavigation.Abstractions;
using FierceStukCloud.Pc.Mvvm.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FierceStukCloud.Pc.Mvvm.ViewModels.PageVMs
{
    public class HomePageVM : BaseViewModel, INavigatedToAware
    {
        public HomePageVM(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void OnNavigatedTo(object arg)
        {

        }
    }
}
