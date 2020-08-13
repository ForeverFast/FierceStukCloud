using Egor92.MvvmNavigation;
using FierceStukCloud.Core;
using FierceStukCloud.Pc.Mvvm.ViewModels;
using FierceStukCloud.Pc.Mvvm.ViewModels.PageVMs;
using FierceStukCloud.Pc.Mvvm.Views;
using FierceStukCloud.Pc.Mvvm.Views.Pages;
using System.Windows;

namespace FierceStukCloud.Pc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static User CurrentUser { get; set; }

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

            var window = new MainWindowV();
            var navigationManager = new NavigationManager(window.FrameContent);

            var viewModel = new MainWindowVM(navigationManager);
            window.DataContext = viewModel;

            navigationManager.Register<HomePage>("home", new HomePageVM(navigationManager));
            navigationManager.Register<ReviewPage>("review", new HomePageVM(navigationManager));
            navigationManager.Register<ProfilePage>("profile", new HomePageVM(navigationManager));

            navigationManager.Register<PlaylistPage>("pl", new PlaylistVM(null));

            //navigationManager.Navigate("pl", null, Egor92.MvvmNavigation.Abstractions.NavigateType.Root);
            navigationManager.Navigate("home", null, Egor92.MvvmNavigation.Abstractions.NavigateType.Root);

            window.Show();



            //var AVM = new AutorizationVM(Dispatcher);
            //await DisplayRootRegistry.ShowModalPresentation(AVM);   
        }
    }


  
}
