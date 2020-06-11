using FierceStukCloud_NetCoreLib.Models;
using FierceStukCloud_NetCoreLib.Models.MusicContainers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
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
            try
            {
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
                            Year = Convert.ToUInt32(SQL["Year"]),
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
            catch (Exception ex)
            {
                connection.Close();
                return null;
            }
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
            try
            {
                if (Songs == null)
                    await GetSongsFromLocalDB();

                foreach (var item in Songs)
                {
                    var title = item.LocalURL.Substring(0, item.LocalURL.LastIndexOf('\\'));
                    title = title.Substring(title.LastIndexOf('\\')+1);
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
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<LocalFolder> AddLocalFoldersFromPC(string path)
        {

            var temp = new LocalFolder()
            {
                Title = path.Substring(path.IndexOf('\\') + 1),
                LocalURL = path
            };

            try
            {
                connection.Open();
                SQLiteCommand CMD;

                string[] tempMas = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);

                int ID_Counter = 0;
                string str = "";
                foreach (var item in tempMas)
                {
                    str = Path.GetFileName(item);
                    string[] TS = str.Split(new string[] { " - ", "_-_" }, StringSplitOptions.RemoveEmptyEntries);
                    TagLib.File file_TAG = TagLib.File.Create(item);

                    temp.Songs.Add(new Song()
                    {
                        ID = ID_Counter,

                        Author = TS[0],
                        Title = TS[1],
                        Album = file_TAG.Tag.Album,
                        Duration = file_TAG.Properties.Duration.ToString(@"mm\:ss"),
                        Year = file_TAG.Tag.Year,

                        PlayListNames = "",
                        LocalURL = item,
                        UserLogin = "",

                        OnServer = false,
                        OnPC = true
                    });
                    ID_Counter++;

                    CMD = connection.CreateCommand();
                    CMD.CommandText = $"INSERT INTO Songs (LocalID,         Author,    Title,       Album,      Duration, Year," +
                                                              $"PlayListNames,   LocalURL,  UserLogin,   OnServer,   OnPC)" +
                                                      $" VALUES(@localID,       @author,   @title,       @album,    @duration, @year," +
                                                              $"@playListNames, @localURL, @userLogin,   @onServer, @onPC);";

                    CMD.Parameters.Add(new SQLiteParameter("@localID", ID_Counter));

                    CMD.Parameters.Add(new SQLiteParameter("@author", TS[0]));
                    CMD.Parameters.Add(new SQLiteParameter("@title", TS[1]));
                    CMD.Parameters.Add(new SQLiteParameter("@album", file_TAG.Tag.Album));
                    CMD.Parameters.Add(new SQLiteParameter("@duration", file_TAG.Properties.Duration.ToString(@"mm\:ss")));
                    CMD.Parameters.Add(new SQLiteParameter("@year", file_TAG.Tag.Year));

                    CMD.Parameters.Add(new SQLiteParameter("@playListNames", ""));
                    CMD.Parameters.Add(new SQLiteParameter("@localURL", item));
                    CMD.Parameters.Add(new SQLiteParameter("@userLogin", ""));

                    CMD.Parameters.Add(new SQLiteParameter("@onServer",false));
                    CMD.Parameters.Add(new SQLiteParameter("@onPC", true));
                    await CMD.ExecuteNonQueryAsync();
                }

                connection.Close();

                return temp;
            }
            catch (Exception ex)
            {
                connection.Close();

                //ErrorPlayListEvent?.Invoke(ex.ToString());
                
                return null;
            }
        }









        public MWM_LocalDB(SQLiteConnection connection)
        {
            this.connection = connection;
        }
    }
}
