using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using FierceStukCloud_Mobile.Services;
using FierceStukCloud_Mobile.Views;
using FierceStukCloud_Mobile.MVVM.Views;
using FierceStukCloud_NetStandardLib.Models;

namespace FierceStukCloud_Mobile
{
    public partial class App : Application
    {

        public static User CurrentUser { get; set; }

        //http://localhost:52828/
        //http://fiercestukcloud.life/
        public static string CurSiteLing = "http://fiercestukcloud.life/";

        public App()
        {
            InitializeComponent();

            //DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new AuthenticationV());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
