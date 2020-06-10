using FierceStukCloud_NetCoreLib.Models;
using FierceStukCloud_NetCoreLib.Models.MusicContainers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FierceStukCloud_PC.MVVM.Models.Modules
{
    public class MWM_LocalDB
    {
        private SQLiteConnection connection { get; }

        private List<Song> Songs { get; set; }



        public async Task<List<Song>> GetSongsFromLocalDB()
        {
            var temp = new List<Song>();
            connection.Open();

            SQLiteCommand CMD = connection.CreateCommand();
            CMD.CommandText = "SELECT * FROM Songs";
            SQLiteDataReader SQL = (SQLiteDataReader)await CMD.ExecuteReaderAsync();

            if (SQL.HasRows)
            {
                while (SQL.Read())
                {
                    temp.Add(new Song()
                    {
                        ID = Convert.ToInt32(SQL["ID"]),
                        Author = SQL["Author"].ToString(),
                        Title = SQL["Title"].ToString(),
                        Album = SQL["Album"].ToString(),
                        Year = Convert.ToInt32(SQL["Year"]),
                        Duration = SQL["Album"].ToString(),

                        PlayListNames = SQL["PlayListNames"].ToString(),
                        LocalURL = SQL["LocalURL"].ToString(),
                        UserLogin = SQL["UserLogin"].ToString(),

                        OnServer = Convert.ToBoolean(SQL["OnServer"]),
                        OnPC = Convert.ToBoolean(SQL["OnPC"])
                    });
                }
            }



            connection.Close();
            Songs = temp;
            return temp;

        }

        public async void SaveSongToLocalDB(Song song)
        {
            connection.Open();
            SQLiteCommand cmd = connection.CreateCommand();



            connection.Close();
        }




        public async Task<List<LocalFolder>> GetLocalFoldersFromLocalDB()
        {
            var temp = new List<LocalFolder>();

            if (Songs == null)
                await GetSongsFromLocalDB();

            foreach (var item in Songs)
            {
                var title = item.LocalURL.Substring(item.LocalURL.IndexOf('\\') + 1);
                var path = item.LocalURL.Substring(0, item.LocalURL.IndexOf('\\'));

                var LF = temp.Find(x => x.LocalURL == path);
                if (LF != null)
                {
                    item.LocalID = LF.Songs.Count + 1;
                    LF.Songs.Add(item);
                }
                else
                {
                    temp.Add(new LocalFolder()
                    {
                        Title = title,
                        LocalURL = path
                    });
                    item.LocalID = temp.Last().Songs.Count + 1;
                    temp.Last().Songs.Add(item);
                }
            }
            return temp;
        }











        public MWM_LocalDB(SQLiteConnection connection)
        {
            this.connection = connection;
        }
    }
}
