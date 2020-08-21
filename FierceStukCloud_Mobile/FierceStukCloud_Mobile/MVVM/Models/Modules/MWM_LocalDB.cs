using FierceStukCloud_NetStandardLib.Models;
using FierceStukCloud_NetStandardLib.Models.MusicContainers;
//using FierceStukCloud_NetStandardLib.Services.MusicTransromations.Tags;
//using static FierceStukCloud_NetStandardLib.Services.Extension.DialogService;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;

namespace FierceStukCloud_Mobile.MVVM.Models.Modules
{
    public class MWM_LocalDB
    {
        private SQLiteConnection connection { get; }

        public List<Song> LocalSongs { get; private set; }


        #region Получение данных их БД

        /// <summary>
        /// Получение песен из локальной БД
        /// </summary>
        /// <returns></returns>
        public List<Song> GetSongsFromLocalDB()
        {
            var temp = new List<Song>();
            try
            {
                connection.Open();

                SQLiteCommand CMD = connection.CreateCommand();
                CMD.CommandText = "SELECT * FROM Songs";
                SQLiteDataReader SQL = CMD.ExecuteReader();

                if (SQL.HasRows)
                {
                    while (SQL.Read())
                    {
                        temp.Add(new Song()
                        {
                            Id = Convert.ToInt32(SQL["ID"]),
                            LocalID = Convert.ToInt32(SQL["LocalID"]),
                            Author = SQL["Author"].ToString(),
                            Title = SQL["Title"].ToString(),
                            Album = SQL["Album"].ToString(),
                            Year = Convert.ToUInt32(SQL["Year"]),
                            Duration = SQL["Album"].ToString(),

                            PlayListNames = SQL["PlayListNames"].ToString(),
                            LocalUrl = SQL["LocalURL"].ToString(),
                            UserLogin = SQL["UserLogin"].ToString(),

                            OnServer = Convert.ToBoolean(SQL["OnServer"]),
                            OnPC = Convert.ToBoolean(SQL["OnPC"]),

                            OptionalInfo = SQL["OptionalInfo"].ToString()
                        });
                    }
                }

                connection.Close();
                return temp;
            }
            catch (Exception)
            {
                connection.Close();
                return null;
            }
        }


        #endregion


        #region Преобразование

        /// <summary>
        /// Создание папок из списка песен
        /// </summary>
        /// <returns></returns>
        public List<LocalFolder> GetListLocalFolders()
        {
            var temp = new List<LocalFolder>();
            LocalSongs = new List<Song>();
            try
            {
                foreach (var item in GetSongsFromLocalDB())
                {
                    if(item.OptionalInfo == "LF")
                    {
                        LocalSongs.Add(item);
                        continue;
                    }

                    var path = item.LocalUrl.Substring(0, item.LocalUrl.LastIndexOf('\\'));

                    var LF = temp.Find(x => x.LocalURL == path);
                    if (LF != null)
                    {
                        item.LocalID = LF.Songs.Count + 1;
                        LF.Songs.Add(item);
                    }
                    else
                    {
                        var title = item.LocalUrl.Substring(0, item.LocalUrl.LastIndexOf('\\'));
                        title = title.Substring(title.LastIndexOf('\\') + 1);

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
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        // Отключено
        #region Добавление/Удаление папок

        ///// <summary>
        ///// Добавление папки в приложение
        ///// </summary>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //public LocalFolder AddLocalFoldersFromPC(string path)
        //{

        //    var temp = new LocalFolder()
        //    {
        //        Title = path.Substring(path.IndexOf('\\') + 1),
        //        LocalURL = path
        //    };

        //    try
        //    {
        //        connection.Open();
        //        SQLiteCommand CMD;

        //        string[] tempMas = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);

        //        int ID_Counter = 0;
        //        int ID_GlobalCounter = this.GetDBSongsCount();

        //        string str = "";
        //        foreach (var item in tempMas)
        //        {
        //            str = Path.GetFileName(item);
        //            TagLib.File file_TAG = TagLib.File.Create(item);
        //            Music_AuthorAndTitleCheck MAaTC = new Music_AuthorAndTitleCheck(str, file_TAG);

        //            temp.Songs.Add(new Song()
        //            {
        //                ID = ID_GlobalCounter++,
        //                LocalID = ID_Counter++,

        //                Author = MAaTC.Author,
        //                Title = MAaTC.Title,
        //                Album = file_TAG.Tag.Album,
        //                Duration = file_TAG.Properties.Duration.ToString(@"mm\:ss"),
        //                Year = file_TAG.Tag.Year,

        //                PlayListNames = "",
        //                LocalURL = item,
        //                UserLogin = "",

        //                OnServer = false,
        //                OnPC = true,

        //                OptionalInfo = ""
        //            });


        //            CMD = connection.CreateCommand();
        //            CMD.CommandText = $"INSERT INTO Songs (LocalID,         Author,    Title,       Album,      Duration,       Year," +
        //                                                      $"PlayListNames,   LocalURL,  UserLogin,   OnServer,   OnPC,      OptionalInfo)" +
        //                                              $" VALUES(@localID,       @author,   @title,       @album,    @duration, @year," +
        //                                                      $"@playListNames, @localURL, @userLogin,   @onServer, @onPC,     @optionalInfo);";

        //            CMD.Parameters.Add(new SQLiteParameter("@localID", ID_Counter));

        //            CMD.Parameters.Add(new SQLiteParameter("@author", MAaTC.Author));
        //            CMD.Parameters.Add(new SQLiteParameter("@title", MAaTC.Title));
        //            CMD.Parameters.Add(new SQLiteParameter("@album", file_TAG.Tag.Album));
        //            CMD.Parameters.Add(new SQLiteParameter("@duration", file_TAG.Properties.Duration.ToString(@"mm\:ss")));
        //            CMD.Parameters.Add(new SQLiteParameter("@year", file_TAG.Tag.Year));

        //            CMD.Parameters.Add(new SQLiteParameter("@playListNames", ""));
        //            CMD.Parameters.Add(new SQLiteParameter("@localURL", item));
        //            CMD.Parameters.Add(new SQLiteParameter("@userLogin", ""));

        //            CMD.Parameters.Add(new SQLiteParameter("@onServer",false));
        //            CMD.Parameters.Add(new SQLiteParameter("@onPC", true));

        //            CMD.Parameters.Add(new SQLiteParameter("@optionalInfo", ""));
        //            CMD.ExecuteNonQuery();
        //        }

        //        connection.Close();

        //        //Temp = temp;
        //        return temp;
        //    }
        //    catch (Exception)
        //    {
        //        connection.Close();

        //        //ErrorPlayListEvent?.Invoke(ex.ToString());
        //        //Temp = null;
        //        return null;
        //    }
        //    //ShowMessage(Thread.CurrentThread.ManagedThreadId.ToString());
        //}

        ///// <summary>
        ///// Удаление папки в приложении
        ///// </summary>
        ///// <param name="localFolder"></param>
        ///// <returns></returns>
        //public LocalFolder DeleteLocalFolderFromApp(LocalFolder localFolder)
        //{
        //    try
        //    {
        //        connection.Open();
        //        SQLiteCommand CMD;

        //        foreach (var item in localFolder.Songs)
        //        {
        //            CMD = connection.CreateCommand();
        //            CMD.CommandText = $"DELETE FROM Songs WHERE ID = {item.ID}";
        //            CMD.ExecuteNonQuery();
        //        }

        //        //DeletePlayListEvent?.Invoke(DL);

        //        connection.Close();
        //        return localFolder;
        //    }
        //    catch (Exception)
        //    {
        //        //ErrorPlayListEvent?.Invoke(ex.ToString());
        //        return null;
        //    }
        //    //ShowMessage(Thread.CurrentThread.ManagedThreadId.ToString());
        //}

        #endregion

        // Отключено
        #region Добавление/Удаление песен

        //public Song AddSongFromPC(string path)
        //{
        //    try
        //    {
        //        connection.Open();
        //        SQLiteCommand CMD;

        //        var str = Path.GetFileName(path);
        //        TagLib.File file_TAG = TagLib.File.Create(path);
        //        Music_AuthorAndTitleCheck MAaTC = new Music_AuthorAndTitleCheck(str, file_TAG);

        //        int ID_Counter = GetDBLocalSongsCount();
        //        int ID_GlobalCounter = this.GetDBSongsCount() + 1;

        //        Song temp = new Song()
        //        {
        //            ID = ID_GlobalCounter,
        //            LocalID = ID_Counter,

        //            Author = MAaTC.Author,
        //            Title = MAaTC.Title,
        //            Album = file_TAG.Tag.Album,
        //            Duration = file_TAG.Properties.Duration.ToString(@"mm\:ss"),
        //            Year = file_TAG.Tag.Year,

        //            PlayListNames = "",
        //            LocalURL = path,
        //            UserLogin = "",

        //            OnServer = false,
        //            OnPC = true,

        //            OptionalInfo = "LF"
        //        };


        //        CMD = connection.CreateCommand();
        //        CMD.CommandText = $"INSERT INTO Songs (      LocalID,        Author,    Title,       Album,      Duration,  Year," +
        //                                                   $"PlayListNames,  LocalURL,  UserLogin,   OnServer,   OnPC,      OptionalInfo)" +
        //                                          $" VALUES(@localID,       @author,   @title,       @album,    @duration, @year," +
        //                                                  $"@playListNames, @localURL, @userLogin,   @onServer, @onPC,     @optionalInfo);";

        //        CMD.Parameters.Add(new SQLiteParameter("@localID", ID_Counter));

        //        CMD.Parameters.Add(new SQLiteParameter("@author", MAaTC.Author));
        //        CMD.Parameters.Add(new SQLiteParameter("@title", MAaTC.Title));
        //        CMD.Parameters.Add(new SQLiteParameter("@album", file_TAG.Tag.Album));
        //        CMD.Parameters.Add(new SQLiteParameter("@duration", file_TAG.Properties.Duration.ToString(@"mm\:ss")));
        //        CMD.Parameters.Add(new SQLiteParameter("@year", file_TAG.Tag.Year));

        //        CMD.Parameters.Add(new SQLiteParameter("@playListNames", ""));
        //        CMD.Parameters.Add(new SQLiteParameter("@localURL", path));
        //        CMD.Parameters.Add(new SQLiteParameter("@userLogin", ""));

        //        CMD.Parameters.Add(new SQLiteParameter("@onServer", false));
        //        CMD.Parameters.Add(new SQLiteParameter("@onPC", true));

        //        CMD.Parameters.Add(new SQLiteParameter("@optionalInfo", "LF"));
        //        CMD.ExecuteNonQuery();


        //        connection.Close();

        //        return temp;
        //    }
        //    catch (Exception)
        //    {
        //        connection.Close();

        //        //ErrorPlayListEvent?.Invoke(ex.ToString());
        //        return null;
        //    }
        //}

        //public Song DeleteSongFromApp(Song song)
        //{
        //    try
        //    {
        //        connection.Open();
        //        SQLiteCommand CMD;

        //        CMD = connection.CreateCommand();
        //        CMD.CommandText = $"DELETE FROM Songs WHERE ID = {song.ID}";
        //        CMD.ExecuteNonQuery();

        //        connection.Close();
        //        return song;
        //    }
        //    catch (Exception)
        //    {
        //        //ErrorPlayListEvent?.Invoke(ex.ToString());
        //        return null;
        //    }

        //}

        #endregion


        #region Дополнительные методы


        private int GetDBSongsCount()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SQLiteCommand CMD;

                    CMD = connection.CreateCommand();
                    CMD.CommandText = "select seq from sqlite_sequence where name ='Songs'";
                    return Convert.ToInt32(CMD.ExecuteScalar());
                }
                else
                    return 0;
            }
            catch(Exception)
            {
                return 0;
            }
        }

        private int GetDBLocalSongsCount()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SQLiteCommand CMD;

                    CMD = connection.CreateCommand();
                    CMD.CommandText = "SELECT count(*) FROM Songs WHERE OptionalInfo='LF'";
                    return Convert.ToInt32(CMD.ExecuteScalar());
                }
                else
                    return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion


        public MWM_LocalDB(SQLiteConnection connection)
            => this.connection = connection;    
    }
}



/* OLD CODE - поиск автора и названия. Теперь этим занимается класс Music_AuthorAndTitleCheck
  //string[] TS = new string[2];
  // = str.Split(new string[] { " - ", "_-_" }, StringSplitOptions.RemoveEmptyEntries);
 */
