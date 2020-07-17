using FierceStukCloud_NetStandardLib.Models;
using FierceStukCloud_NetCoreLib.Services;
using FierceStukCloud_PC.MVVM.ViewModels;
using FierceStukCloud_PC.MVVM.Views;
using FierceStukCloud_PC.MVVM.Views.TestView;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NLog;
using static System.Diagnostics.Debug;

namespace FierceStukCloud_PC
{
    public partial class App : Application
    {
        public static User CurrentUser { get; set; }
        public static DisplayRootRegistry DisplayRootRegistry { get; private set; }

        public static SQLiteConnection Connection { get; set; }

        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        //http://localhost:52828/
        //http://fiercestukcloud.life/
        public static string CurSiteLing = "http://fiercestukcloud.life/";

        public App()
        {
            Log.Debug("Запуск приложения...");
            WriteLine("Запуск приложения...");
            try
            {
                #region Локальная БД
                string dbFileName = "fscLocalDB.db";

                if (!File.Exists(dbFileName))
                    SQLiteConnection.CreateFile(dbFileName);

                Connection = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                Connection.Open();

                SQLiteCommand cmd = App.Connection.CreateCommand();
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Songs (" +
                    "ID             INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "LocalID        INTEGER," +
                    "Author         nvarchar(300)," +
                    "Title          nvarchar(300)," +
                    "Album          nvarchar(300)," +
                    "Duration       nvarchar(20)," +
                    "Year           nvarchar(20)," +
                    "PlayListNames  nvarchar(4000)," +
                    "LocalURL       nvarchar(2000), " +
                    "UserLogin      nvarchar(200)," +
                    "OnServer       BOOLEAN," +
                    "OnPC           BOOLEAN," +
                    "OptionalInfo   nvarchar(200))";
                cmd.ExecuteNonQuery();
                Connection.Close();
                #endregion


            }
            catch (Exception)
            {

            }

            #region Настройка окон
            DisplayRootRegistry = new DisplayRootRegistry();
            DisplayRootRegistry.RegisterWindowType<AutorizationVM, AuthorizationV>();
            DisplayRootRegistry.RegisterWindowType<MainWindowVM, MainWindowV>();
            #endregion
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
           
            var AVM = new AutorizationVM(Dispatcher);
            await DisplayRootRegistry.ShowModalPresentation(AVM);
            
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Debug("Завершение работы приложения...");

            Zidium.Api.Client.Instance.EventManager.Flush();
            Zidium.Api.Client.Instance.WebLogManager.Flush();

            base.OnExit(e);
        }
    }
}

