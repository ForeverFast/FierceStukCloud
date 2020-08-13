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

        public List<Song> AllSongs { get; set; }
        public PlayList Favourites { get; }
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
                        OnDevice = true,

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
                    AllSongs = output.ToList();

                    foreach (var item in await cnn.QueryAsync<Album>("SELECT * FROM Albums"))
                        Albums.Add(item);
                    foreach (var item in await cnn.QueryAsync<LocalFolder>("SELECT * FROM LocalFolders"))
                        LocalFolders.Add(item);
                    foreach (var item in await cnn.QueryAsync<PlayList>("SELECT * FROM PlayLists"))
                        PlayLists.Add(item);


                    foreach (var item in AllSongs)
                        await SongIntegrating(item, cnn);
                }
                catch (Exception)
                {

                }
            }
        }

        #region Методы интеграции


        private async Task SongIntegrating(Song song, IDbConnection cnn)
        {
            await SearchSongAlbum(song, cnn);
            await SearchSongLocalFolder(song, cnn);
            await SearchSongPlayLists(song, cnn);
        }

        private async Task SearchSongAlbum(Song song, IDbConnection cnn)
        {
            try
            {
                var Album = Albums.FirstOrDefault(x => x.Title.ToLower() == song.Album.ToLower() &&
                                                      x.Author.ToLower() == song.Author.ToLower());
                if (Album != null)
                {
                    if (Album.Songs == null)
                        Album.Songs = new LinkedList<Song>();
                    Album.Songs.AddLast(song);
                }
                else
                {
                    if (!string.IsNullOrEmpty(song.Album) && song.Album != "Неизвестный")
                    {
                        var NewAlbum = new Album()
                        {
                            Id = Guid.NewGuid().ToString(),
                            //Title
                            Author = song.Author,
                            UserLogin = song.UserLogin,
                            Songs = new LinkedList<Song>()
                        };
                        NewAlbum.Title = song.Album;

                        string sql = $"INSERT INTO Albums (Id,  Title,  Author,  UserLogin)" +
                                                $" VALUES(@Id, @Title, @Author, @UserLogin);";

                        await cnn.ExecuteAsync(sql, NewAlbum);

                        
                        NewAlbum.Songs.AddLast(song);
                        Albums.Add(NewAlbum);
                       
                    }
                    else
                    {
                        var NAlbum = Albums.FirstOrDefault(x => x.Title == "Неизвестный");
                        if (NAlbum != null)
                        {
                            if (NAlbum.Songs == null)
                                NAlbum.Songs = new LinkedList<Song>();
                            NAlbum.Songs.AddLast(song);
                        } 
                        else
                        {
                            NAlbum = new Album()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Title = "Неизвестный",
                                Author = song.Author,
                                UserLogin = song.UserLogin,
                                Songs = new LinkedList<Song>()
                            };

                            string sql = $"INSERT INTO Albums (Id,  Title,  Author,  UserLogin)" +
                                             $" VALUES(@Id, @Title, @Author, @UserLogin);";

                            await cnn.ExecuteAsync(sql, NAlbum);

                            NAlbum.Songs.AddLast(song);
                            Albums.Add(NAlbum);
                        }
                    }

                    

                }
            }
            catch (Exception)
            {

            }
        }

        private async Task SearchSongLocalFolder(Song song, IDbConnection cnn)
        {
            try
            {
                var FolderPath = GetSongFolderPath(song.LocalUrl);

                var LF = LocalFolders.FirstOrDefault(x => x.LocalUrl == FolderPath);
                if (LF != null)
                {
                    if (LF.Songs == null)
                        LF.Songs = new LinkedList<Song>();
                    LF.Songs.AddLast(song);
                }
                else
                {
                    var NewLF = new LocalFolder()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = FolderPath.Remove(0, FolderPath.IndexOf('\\') + 1),
                        LocalUrl = FolderPath,
                        UserLogin = song.UserLogin,
                        OnDevice = true,
                        OnServer = false,
                        Songs = new LinkedList<Song>()
                    };

                    string sql = $"INSERT INTO LocalFolders (Id,  Title,  LocalUrl,  UserLogin,  OnServer,   OnDevice)" +
                                                  $" VALUES(@Id, @Title, @LocalUrl, @UserLogin, @OnServer,  @OnDevice);";

                    await cnn.ExecuteAsync(sql, NewLF);

                    NewLF.Songs.AddLast(song);
                    LocalFolders.Add(NewLF);
                }
               
            }
            catch (Exception)
            {
                
            }
        }

        private async Task SearchSongPlayLists(Song song, IDbConnection cnn)
        {
            try
            {
                if (song.PlayLists != null)
                    foreach (var playList in song.PlayLists)
                    {
                        var PL = PlayLists.FirstOrDefault(x => x.Id == playList);
                        if (PL != null)
                        {
                            if (PL.Songs == null)
                                PL.Songs = new LinkedList<Song>();
                            PL.Songs.AddLast(song);
                        }
                        else
                        {
                            var NewPL = new PlayList()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Title = playList,
                                CreationDate = DateTime.Now,
                                UserLogin = song.UserLogin,
                                OnDevice = true,
                                OnServer = false,
                                Songs = new LinkedList<Song>()
                            };

                            string sql = $"INSERT INTO PlayLists (Id,  Title,  UserLogin,  OnServer,   OnPC)" +
                                                       $" VALUES(@Id, @Title, @UserLogin, @OnServer,  @OnPC);";

                            await cnn.ExecuteAsync(sql, NewPL);

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

                //var temp = new LocalFolder()
                //{
                //    Title = path.Substring(path.IndexOf('\\') + 1),
                //    LocalUrl = path,
                //    Songs = new LinkedList<Song>()
                //};
                //LocalFolders.Add(temp);

                foreach (var item in tempMas)
                {
                    var song = await AddSong(item, "");
                    await SongIntegrating(song, null);
                    
                }

                
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

        private string GetSongFolderPath(string songPath)
            => songPath.Remove(songPath.LastIndexOf('\\'));
        




        #endregion


        public DataService(List<Song> allSongs,
                           PlayList favourites,
                           ObservableCollection<Album> albums,
                           ObservableCollection<LocalFolder> localFolders,
                           ObservableCollection<PlayList> playLists,
                           User user)
        {
            SqlMapper.AddTypeHandler(typeof(List<string>), new DictTypeHandler());

            this.AllSongs = allSongs;
            this.Favourites = favourites;
            this.Albums = albums;
            this.LocalFolders = localFolders;
            this.PlayLists = playLists;

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