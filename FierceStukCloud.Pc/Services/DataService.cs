using Dapper;
using FierceStukCloud.Core;
using FierceStukCloud.Core.MusicPlayerModels;
using FierceStukCloud.Core.MusicPlayerModels.MusicContainers;
using FierceStukCloud.Core.Other;
using FierceStukCloud.Wpf.Services;
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
using static Dapper.SqlMapper;

namespace FierceStukCloud.Pc.Services
{
    public class DataService : IDataService
    {
        private User _user { get; }


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

                    //int ID_GlobalCounter = this.GetDBSongsCount(cnn) + 1;

                    Song temp = new Song()
                    {
                        Id = Guid.NewGuid().ToString(),

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

        #region Получение информации

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
                    //song.LocalId.Add(new KeyValuePair<IMusicContainer, int>(Album, Album.Songs.Count));
                    Album.Songs.AddLast(song);
                }
                else
                {
                    var NewAlbum = new Album()
                    {
                        Author = song.Author,
                        Songs = new LinkedList<Song>()
                    };

                    if (!string.IsNullOrEmpty(song.Album))
                        NewAlbum.Title = song.Album;
                    else
                        NewAlbum.Title = "Неизвестный";

                 
                    NewAlbum.Songs.AddLast(song);
                    Albums.Add(NewAlbum);
                }
            }
            catch (Exception)
            {

            }
        }

        public void SearchSongLocalFolder(Song song)
        {
            try
            {
                if (song.OptionalInfo == "LF")
                {
                    LocalSongs.Songs.AddLast(song);
                    return;
                }


                var itemPath = song.LocalUrl;
                var path = itemPath.Remove(itemPath.LastIndexOf('\\'));

                var LF = LocalFolders.FirstOrDefault(x => x.LocalUrl == path);
                if (LF != null)
                {
                    LF.Songs.AddLast(song);
                }
                else
                {
                    var NewLF = new LocalFolder()
                    {
                        Title = path.Remove(0, path.IndexOf('\\') + 1),
                        LocalUrl = path,
                        Songs = new LinkedList<Song>()
                    };

                    NewLF.Songs.AddLast(song);
                    LocalFolders.Add(NewLF);
                }
            }
            catch (Exception)
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
                            PL.Songs.AddLast(song);
                        }
                        else
                        {
                            var NewPL = new PlayList()
                            {
                                Title = playList,
                                UserLogin = song.UserLogin,
                                Songs = new LinkedList<Song>()
                            };

                           
                            NewPL.Songs.AddLast(song);
                            PlayLists.Add(NewPL);
                        }
                    }
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #endregion



        public async Task<LocalFolder> AddLocalFolder(string path)
        {
          

            try
            {
                string[] tempMas = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);

                if (tempMas.Count() == 0)
                {
                    return null;
                }

                var temp = new LocalFolder()
                {
                    Title = path.Substring(path.IndexOf('\\') + 1),
                    LocalUrl = path,
                    Songs = new LinkedList<Song>()
                };
                LocalFolders.Add(temp);

                foreach (var item in tempMas)
                {
                    var song = await AddSong(item, "");
                    SongIntegrating(song);
                    //temp.Songs.AddLast(song);
                }

                return temp;
            }
            catch (Exception)
            {
                return null;
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


      

        #region Дополнительные методы

        private int GetDBSongsCount(IDbConnection cnn)
            => Convert.ToInt32(cnn.ExecuteScalar("select seq from sqlite_sequence where name ='Songs'"));






        #endregion


        public DataService(LocalFolder LocalSongs,
                           ObservableCollection<Album> Albums,
                           ObservableCollection<LocalFolder> LocalFolders,
                           ObservableCollection<PlayList> PlayLists,
                           User user)
        {
            SqlMapper.AddTypeHandler(typeof(List<string>), new DictTypeHandler());

            this.LocalSongs = LocalSongs;
            this.Albums = Albums;
            this.LocalFolders = LocalFolders;
            this.PlayLists = PlayLists;

            _user = user;
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

//song.LocalId.Add(new KeyValuePair<IMusicContainer, int>(NewPL, NewPL.Songs.Count));