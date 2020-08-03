using Dapper;
using FierceStukCloud.Core;
using FierceStukCloud.Core.MusicPlayerModels;
using FierceStukCloud.Core.MusicPlayerModels.MusicContainers;
using FierceStukCloud.Core.Services;
using FierceStukCloud_NetCoreLib.Services.MusicTransromations.Tags;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using static Dapper.SqlMapper;

namespace FierceStukCloud_PC.MVVM.Models.Modules
{
    public class DataService : IDataService
    {
        private User _user { get; }
        private Dispatcher Dispatcher { get; }


        public LocalFolder LocalSongs { get; }
        public ObservableCollection<Album> Albums { get; }
        public ObservableCollection<LocalFolder> LocalFolders { get; }
        public ObservableCollection<PlayList> PlayLists { get; }


        public async Task<Song> AddSong(string path, string optionalInfo = "LF")
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                try
                {
                    TagLib.File file_TAG = TagLib.File.Create(path);
                    Music_AuthorAndTitleCheck MAaTC = new Music_AuthorAndTitleCheck(Path.GetFileName(path), file_TAG);

                    int ID_GlobalCounter = this.GetDBSongsCount(cnn) + 1;

                    Song temp = new Song()
                    {
                        Id = ID_GlobalCounter,

                        Author = MAaTC.Author,
                        Title = MAaTC.Title,
                        //Album
                        Year = file_TAG.Tag.Year,

                        Duration = file_TAG.Properties.Duration.ToString(@"mm\:ss"),
                        LocalUrl = path,

                        UserLogin = _user.Login,
                        OnServer = false,
                        OnPC = true,

                        OptionalInfo = optionalInfo
                    };

                    if (!string.IsNullOrEmpty(file_TAG.Tag.Album))
                        temp.Album = file_TAG.Tag.Album;
                    else
                        temp.Album = "Неизвестный";

                    string sql = $"INSERT INTO Songs (Author,    Title,       Album,      Duration,  Year," +
                                                    $"PlayLists,  LocalURL,  UserLogin,   OnServer,   OnPC,      OptionalInfo)" +
                                           $" VALUES(@author,   @title,       @album,    @duration, @year," +
                                                   $"@playLists, @localURL, @userLogin,   @onServer, @onPC,     @optionalInfo);";

                    await cnn.ExecuteAsync(sql, temp);

                    return temp;
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                }
            }
        }

        public async Task<bool> RemoveSong(Song song)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                try
                {
                    string sql = $"DELETE FROM Songs WHERE Id = @Id";
                    await cnn.ExecuteAsync(sql, song);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public async Task GetData()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                try
                {          
                    var output = await cnn.QueryAsync<Song>("SELECT * FROM Songs");
                    var tempSongs = output.ToList();

                    foreach (var item in tempSongs)
                        SongIntegrating(item);
                }
                catch (Exception)
                {

                }
            }
        }


        public async Task<LocalFolder> AddLocalFolder(string path)
        {
            var temp = new LocalFolder()
            {
                Title = path.Substring(path.IndexOf('\\') + 1),
                LocalUrl = path
            };


            try
            {
                string[] tempMas = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);

                foreach (var item in tempMas)
                {
                    var song = await AddSong(item, "");
                    song.LocalId.Add(new KeyValuePair<IMusicContainer, int>(temp, temp.Songs.Count));
                    temp.Songs.Add(song);
                }

                return temp;
            }
            catch (Exception)
            {
                return temp;
            }

        }

        public async Task<bool> RemoveLocalFolder(LocalFolder localFolder)
        {
            try
            {
                foreach (var item in localFolder.Songs)
                {
                    //CMD = connection.CreateCommand();
                    //CMD.CommandText = $"DELETE FROM Songs WHERE ID = {item.Id}";
                    //CMD.ExecuteNonQuery();
                }
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string LoadConnectionString(string id = "SQLite")
            => ConfigurationManager.ConnectionStrings[id].ConnectionString;


        #region Методы сортировки

        public void SongIntegrating(Song song)
        {
            SearchSongAlbum(song);
            SearchSongLocalFolder(song);
            SearchSongPlayLists(song);
        }

        public void SearchSongAlbum(Song song)
        {
            try
            {
                var Album = Albums.FirstOrDefault(x => x.Title.ToLower() == song.Album.ToLower() &&
                                              x.Author.ToLower() == song.Author.ToLower());
                if (Album != null)
                {
                    song.LocalId.Add(new KeyValuePair<IMusicContainer, int>(Album, Album.Songs.Count));
                    Album.Songs.Add(song);
                }
                else
                {
                    var NewAlbum = new Album()
                    {
                        Author = song.Author,
                        //Songs = new ImageAsyncCollection<ImageAsync<Song>>(Dispatcher),
                    };

                    

                    if (!string.IsNullOrEmpty(song.Album))
                        NewAlbum.Title = song.Album;
                    else
                        NewAlbum.Title = "Неизвестный";

                    song.LocalId.Add(new KeyValuePair<IMusicContainer, int>(NewAlbum, NewAlbum.Songs.Count));
                    NewAlbum.Songs.Add(song);
                    Albums.Add(NewAlbum);
                }
            }
            catch(Exception)
            {

            }
        }

        public void SearchSongLocalFolder(Song song)
        {
            try
            {
                if (song.OptionalInfo == "LF")
                {
                    song.LocalId.Add(new KeyValuePair<IMusicContainer, int>(LocalSongs, LocalSongs.Songs.Count));
                    LocalSongs.Songs.Add(song);
                    return;
                }


                var itemPath = song.LocalUrl;
                var path = itemPath.Remove(itemPath.LastIndexOf('\\'));

                var LF = LocalFolders.FirstOrDefault(x => x.LocalUrl == path);
                if (LF != null)
                {
                    song.LocalId.Add(new KeyValuePair<IMusicContainer, int>(LF, LF.Songs.Count));
                    LF.Songs.Add(song);
                }
                else
                {
                    var NewLF = new LocalFolder()
                    {
                        Title = path.Remove(0, path.IndexOf('\\') + 1),
                        LocalUrl = path
                    };

                    song.LocalId.Add(new KeyValuePair<IMusicContainer, int>(NewLF, NewLF.Songs.Count));
                    NewLF.Songs.Add(song);
                    LocalFolders.Add(NewLF);
                }
            }
            catch(Exception)
            {

            }
        }

        public void SearchSongPlayLists(Song song)
        {
            try
            {
                if (song.PlayLists != null)
                    foreach (var playList in song.PlayLists)
                    {
                        var PL = PlayLists.FirstOrDefault(x => x.Title == playList);
                        if (PL != null)
                        {
                            song.LocalId.Add(new KeyValuePair<IMusicContainer, int>(PL, PL.Songs.Count));
                            PL.Songs.Add(song);
                        }
                        else
                        {
                            var NewPL = new PlayList()
                            {
                                Title = playList,
                                UserLogin = song.UserLogin
                            };

                            song.LocalId.Add(new KeyValuePair<IMusicContainer, int>(NewPL, NewPL.Songs.Count));
                            NewPL.Songs.Add(song);
                            PlayLists.Add(NewPL);
                        }
                    }
            }
            catch(Exception)
            {

            }
        }

        #endregion

        #region Дополнительные методы

        private int GetDBSongsCount(IDbConnection cnn) 
            => Convert.ToInt32(cnn.ExecuteScalar("select seq from sqlite_sequence where name ='Songs'"));
             





        #endregion


        public DataService(LocalFolder LocalSongs,
                           ObservableCollection<Album> Albums,
                           ObservableCollection<LocalFolder> LocalFolders,
                           ObservableCollection<PlayList> PlayLists,
                           User user, Dispatcher dispatcher)
        {
            SqlMapper.AddTypeHandler(typeof(List<string>), new DictTypeHandler());

            this.LocalSongs = LocalSongs;
            this.Albums = Albums;
            this.LocalFolders = LocalFolders;
            this.PlayLists = PlayLists;

            _user = user;
            this.Dispatcher = dispatcher;
        }
    }
}

public class DictTypeHandler : ITypeHandler
{
    public object Parse(Type destinationType, object value)
    {
        return JsonConvert.DeserializeObject(value.ToString(), destinationType);
    }

    public void SetValue(IDbDataParameter parameter, object value)
    {
        parameter.Value = (value == null)
            ? (object)DBNull.Value
            : JsonConvert.SerializeObject(value);
        parameter.DbType = DbType.String;
    }
}


/* OLD CODE - поиск автора и названия. Теперь этим занимается класс Music_AuthorAndTitleCheck
  //string[] TS = new string[2];
  // = str.Split(new string[] { " - ", "_-_" }, StringSplitOptions.RemoveEmptyEntries);
 */


/*
  CMD = connection.CreateCommand();
                CMD.CommandText = $"INSERT INTO Songs (LocalID,         Author,    Title,       Album,      Duration,       Year," +
                                                          $"PlayListNames,   LocalURL,  UserLogin,   OnServer,   OnPC,      OptionalInfo)" +
                                                  $" VALUES(@localID,       @author,   @title,       @album,    @duration, @year," +
                                                          $"@playListNames, @localURL, @userLogin,   @onServer, @onPC,     @optionalInfo);";

                CMD.Parameters.Add(new SQLiteParameter("@localID", ID_Counter));

                CMD.Parameters.Add(new SQLiteParameter("@author", MAaTC.Author));
                CMD.Parameters.Add(new SQLiteParameter("@title", MAaTC.Title));
                CMD.Parameters.Add(new SQLiteParameter("@album", file_TAG.Tag.Album));
                CMD.Parameters.Add(new SQLiteParameter("@duration", file_TAG.Properties.Duration.ToString(@"mm\:ss")));
                CMD.Parameters.Add(new SQLiteParameter("@year", file_TAG.Tag.Year));

                CMD.Parameters.Add(new SQLiteParameter("@playListNames", ""));
                CMD.Parameters.Add(new SQLiteParameter("@localURL", item));
                CMD.Parameters.Add(new SQLiteParameter("@userLogin", ""));

                CMD.Parameters.Add(new SQLiteParameter("@onServer", false));
                CMD.Parameters.Add(new SQLiteParameter("@onPC", true));

                CMD.Parameters.Add(new SQLiteParameter("@optionalInfo", ""));
                CMD.ExecuteNonQuery();
 */

/*
   CMD = connection.CreateCommand();
                CMD.CommandText = "INSERT INTO LocalFolders (Title, LocalUrl, Songs, UserLogin, OnServer, OnPC) " +
                                                   "VALUES (@title,@localUrl,@songs,@UserLogin,@onServer,@onPC);";

                CMD.Parameters.Add(new SQLiteParameter("@title", temp.Title));
                CMD.Parameters.Add(new SQLiteParameter("@localUrl", temp.LocalUrl));
                CMD.Parameters.Add(new SQLiteParameter("@songs", JsonSerializer.Serialize<List<Song>>(temp.Songs)));

                CMD.Parameters.Add(new SQLiteParameter("@userLogin", User.Login));
                CMD.Parameters.Add(new SQLiteParameter("@onServer", false));
                CMD.Parameters.Add(new SQLiteParameter("@onPC", true));

                CMD.ExecuteNonQuery();

   CMD = connection.CreateCommand();
                    CMD.CommandText = $"INSERT INTO Songs (      Author,    Title,       Album,      Duration,  Year," +
                                                               $"PlayLists,  LocalURL,  UserLogin,   OnServer,   OnPC,      OptionalInfo)" +
                                                      $" VALUES(@author,   @title,       @album,    @duration, @year," +
                                                              $"@playLists, @localURL, @userLogin,   @onServer, @onPC,     @optionalInfo);";

                    CMD.Parameters.Add(new SQLiteParameter("@author", MAaTC.Author));
                    CMD.Parameters.Add(new SQLiteParameter("@title", MAaTC.Title));
                    CMD.Parameters.Add(new SQLiteParameter("@album", file_TAG.Tag.Album));
                    CMD.Parameters.Add(new SQLiteParameter("@duration", file_TAG.Properties.Duration.ToString(@"mm\:ss")));
                    CMD.Parameters.Add(new SQLiteParameter("@year", file_TAG.Tag.Year));

                    CMD.Parameters.Add(new SQLiteParameter("@playLists", ""));
                    CMD.Parameters.Add(new SQLiteParameter("@localURL", path));
                    CMD.Parameters.Add(new SQLiteParameter("@userLogin", User.Login));

                    CMD.Parameters.Add(new SQLiteParameter("@onServer", false));
                    CMD.Parameters.Add(new SQLiteParameter("@onPC", true));

                    CMD.Parameters.Add(new SQLiteParameter("@optionalInfo", "LF"));
                    await CMD.ExecuteNonQueryAsync();



 */

/*
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
 */


/*
 
 if (SQL.HasRows)
                    {
                        while (SQL.Read())
                        {
                            tempSongs.Add(new Song()
                            {
                                ID = Convert.ToInt32(SQL["ID"]),

                                Author = SQL["Author"].ToString(),
                                Title = SQL["Title"].ToString(),
                                Album = SQL["Album"].ToString(),
                                Year = Convert.ToUInt32(SQL["Year"]),

                                Duration = SQL["Duration"].ToString(),
                                LocalUrl = SQL["LocalURL"].ToString(),
                                PlayLists = JsonConvert.DeserializeObject<List<string>>(SQL["PlayLists"].ToString()),

                                UserLogin = SQL["UserLogin"].ToString(),
                                OnServer = Convert.ToBoolean(SQL["OnServer"]),
                                OnPC = Convert.ToBoolean(SQL["OnPC"]),

                                OptionalInfo = SQL["OptionalInfo"].ToString()
                            });
                        }
                    }
 
 */

/*
                    #region Настройки сериализации
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    settings.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
                    settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
                    settings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                    settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                    #endregion
  TagLib.File file_TAG = TagLib.File.Create(item);
                        Music_AuthorAndTitleCheck MAaTC = new Music_AuthorAndTitleCheck(Path.GetFileName(item), file_TAG);

                        temp.Songs.Add(new Song()
                        {
                            Id = ID_GlobalCounter++,

                            Author = MAaTC.Author,
                            Title = MAaTC.Title,
                            Album = file_TAG.Tag.Album,
                            Year = file_TAG.Tag.Year,

                            Duration = file_TAG.Properties.Duration.ToString(@"mm\:ss"),
                            LocalUrl = item,

                            UserLogin = App.CurrentUser.Login,
                            OnServer = false,
                            OnPC = true,

                            OptionalInfo = ""
                        });

                       

                        temp.Songs.Last().LocalId.Add(new KeyValuePair<MusicContainer, int>(temp, temp.Songs.Count));


                        CMD = connection.CreateCommand();
                        CMD.CommandText = $"INSERT INTO Songs (      Author,    Title,       Album,      Duration,  Year," +
                                                                   $"PlayLists,  LocalURL,  UserLogin,   OnServer,   OnPC,      OptionalInfo)" +
                                                          $" VALUES(@author,   @title,       @album,    @duration, @year," +
                                                                  $"@playLists, @localURL, @userLogin,   @onServer, @onPC,     @optionalInfo);";

                        CMD.Parameters.Add(new SQLiteParameter("@author", MAaTC.Author));
                        CMD.Parameters.Add(new SQLiteParameter("@title", MAaTC.Title));
                        CMD.Parameters.Add(new SQLiteParameter("@album", file_TAG.Tag.Album));
                        CMD.Parameters.Add(new SQLiteParameter("@duration", file_TAG.Properties.Duration.ToString(@"mm\:ss")));
                        CMD.Parameters.Add(new SQLiteParameter("@year", file_TAG.Tag.Year));

                        CMD.Parameters.Add(new SQLiteParameter("@playLists", ""));
                        CMD.Parameters.Add(new SQLiteParameter("@localURL", path));
                        CMD.Parameters.Add(new SQLiteParameter("@userLogin", _user.Login));

                        CMD.Parameters.Add(new SQLiteParameter("@onServer", false));
                        CMD.Parameters.Add(new SQLiteParameter("@onPC", true));

                        CMD.Parameters.Add(new SQLiteParameter("@optionalInfo", ""));
                        CMD.ExecuteNonQuery();
 */
