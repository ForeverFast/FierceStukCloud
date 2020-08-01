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

        public string _test;
        public string test
        {
            get => _test;
            set => SetProperty(ref _test, value);
        }

        public HomePageVM(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void OnNavigatedTo(object arg)
        {
            
        }
    }
}
