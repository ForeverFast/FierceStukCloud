using Egor92.MvvmNavigation;
using Egor92.MvvmNavigation.Abstractions;
using FierceStukCloud_NetCoreLib.Commands;
using FierceStukCloud_NetCoreLib.ViewModels;
using FierceStukCloud_NetStandardLib.Models;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FierceStukCloud_PC.MVVM.ViewModels
{
    public class TestVM : BaseViewModel, INavigatedToAware
    {
        private readonly NavigationManager _navigationManager;

       

        public string _test;
        public string test
        {
            get => _test;
            set => SetProperty(ref _test, value);
        }

        public ICommand update { get; }


        private async Task TaskQ(object parameter)
        {
            //Messenger.Default.Send<NavigateArgs>(new NavigateArgs("MVVM/Views/Pages/HomePage.xaml", new Song(), NavigateType.NavigateTo));
            _navigationManager.Navigate(parameter.ToString(),NavigateType.Default);
        }

        public void OnNavigatedTo(object arg)
        {
            
        }

        
        public TestVM()
        {
            update = new AsyncRelayCommand(TaskQ, null);
        }

        public TestVM(NavigationManager navigationManager) : this()
        {
            _navigationManager = navigationManager;
        }
    }
}
