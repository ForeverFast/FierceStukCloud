using Egor92.MvvmNavigation;
using Egor92.MvvmNavigation.Abstractions;
using FierceStukCloud.Core;
using FierceStukCloud.Core.Services;
using FierceStukCloud.Pc.Mvvm.ViewModels;
using FierceStukCloud.Pc.Mvvm.ViewModels.PageVMs;
using FierceStukCloud.Pc.Mvvm.Views;
using FierceStukCloud.Pc.Mvvm.Views.Pages;
using FierceStukCloud.Pc.Services;
using FierceStukCloud.Wpf.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;

namespace FierceStukCloud.Pc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static User CurrentUser { get; set; }

        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }


        //http://localhost:52828/
        //http://fiercestukcloud.life/
        public static string CurSiteLink = "http://fiercestukcloud.life/";

        public App()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CurrentUser = new User() { Login = "ForeverFast" };

            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory());
             //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
            var mainWindow = ServiceProvider.GetRequiredService<MainWindowV>();
            var navigationManager = ServiceProvider.GetRequiredService<INavigationManager>();
            navigationManager.FrameControl = mainWindow.FrameContent;
            var q = ServiceProvider.GetRequiredService<IServiceProvider>();
            navigationManager.Register<HomePage>("home", new HomePageVM(navigationManager));
            navigationManager.Register<ReviewPage>("review", new HomePageVM(navigationManager));
            navigationManager.Register<ProfilePage>("profile", new HomePageVM(navigationManager));

            navigationManager.Navigate("home", NavigateType.Root, null);
            
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMusicStorage, MusicStrorage>();
            services.AddSingleton<ISignalRService, SignalRService>();           
            services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationManager, NavigationManager>();
            services.AddSingleton<IMusicPlayerService, MusicPlayerService>();

            services.AddSingleton(typeof(User));
            services.AddSingleton(typeof(MainWindowVM));
            services.AddSingleton(typeof(MainWindowV));
        }


    }


  
}
