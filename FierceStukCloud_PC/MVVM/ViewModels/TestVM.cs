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
        private readonly INavigationManager _navigationManager;

       

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
            _navigationManager.Navigate("home1");
        }

        public void OnNavigatedTo(object arg)
        {
            test = (string)arg;
        }

        
        public TestVM()
        {
            update = new AsyncRelayCommand(TaskQ, null);
        }

        public TestVM(INavigationManager navigationManager) : this()
        {
            _navigationManager = navigationManager;
        }
    }
}
