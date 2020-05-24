using FierceStukCloud_NetCoreLib.Models;
using FierceStukCloud_NetCoreLib.Services;
using FierceStukCloud_PC.MVVM.ViewModels;
using FierceStukCloud_PC.MVVM.Views;
using FierceStukCloud_PC.MVVM.Views.TestView;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FierceStukCloud_PC
{
    public partial class App : Application
    {
        public static User CurrentUser { get; set; }
        public static DisplayRootRegistry DisplayRootRegistry { get; private set; }

        public App()
        {
            #region Локальная БД

            #endregion

            #region Настройка окон
            DisplayRootRegistry = new DisplayRootRegistry();
            DisplayRootRegistry.RegisterWindowType<AutorizationVM, AuthorizationV>();
            DisplayRootRegistry.RegisterWindowType<MainWindowVM, MainWindowV>();
            #endregion
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //var q = new Test1();
            //q.ShowDialog();

            var AVM = new AutorizationVM();
            await DisplayRootRegistry.ShowModalPresentation(AVM);

        }
    }
}
