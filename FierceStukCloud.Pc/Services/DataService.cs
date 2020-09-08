using Dapper;
using FierceStukCloud.Core;
using FierceStukCloud.Core.Extension;
using FierceStukCloud.Core.Other;
using FierceStukCloud.Core.Services;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static Dapper.SqlMapper;

namespace FierceStukCloud.Pc.Services
{
    public class DataService : IDataService
    {

        private readonly User _user;
        private IMusicStorage _musicStorage;


        #region Получение информации

        public void GetData()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                try
                {
                    var output = cnn.Query<Song>("SELECT * FROM Songs");
                    _musicStorage.AllSongs = new ObservableCollection<Song>(output.ToList());

                    foreach (var item in cnn.Query<Album>("SELECT * FROM Albums"))
                        _musicStorage.Albums.Add(item);
                    foreach (var item in cnn.Query<LocalFolder>("SELECT * FROM LocalFolders"))
                        _musicStorage.LocalFolders.Add(item);
                    foreach (var item in cnn.Query<PlayList>("SELECT * FROM PlayLists"))
                        _musicStorage.PlayLists.Add(item);

                    _musicStorage.AllSongs.AsParallel().ForAll(x => SongIntegrating(x));
                    //foreach (var item in _musicStorage.AllSongs)
                    //{
                    //    SongIntegrating(item);
                    //}
                }
                catch (Exception)
                {

                }
            }
        }


        #region Методы интеграции

        private void SongIntegrating(Song song)
        {
            //var q = Thread.CurrentThread.ManagedThreadId;

            //Parallel.Invoke(
            //    () => SearchSongAlbum(song),
            //    () => SearchSongLocalFolder(song),
            //    () => SearchSongPlayLists(song));

            //List<Action> actions = new List<Action>();
            //actions.Add(() => SearchSongAlbum(song));
            //actions.Add(() => SearchSongLocalFolder(song));
            //actions.Add(() => SearchSongPlayLists(song));

            //actions.AsParallel().ForAll(x => x.Invoke());

            SearchSongAlbum(song);
            SearchSongLocalFolder(song);
            SearchSongPlayLists(song);
        }

        private void SearchSongAlbum(Song song)
        {
            try
            {
                var Album = _musicStorage.Albums.FirstOrDefault(x => x.Title.ToLower() == song.Album.ToLower() &&
                                                                x.Author.ToLower() == song.Author.ToLower());
                if (Album != null)
                {
                    if (Album.Songs == null)
                        Album.Songs = new ObservableLinkedList<Song>();
                    Album.Songs.AddLast(song);
                }
                //else
                //{
                //    if (!string.IsNullOrEmpty(song.Album) && song.Album != "Неизвестный")
                //    {
                //        var NewAlbum = new Album()
                //        {
                //            Id = Guid.NewGuid().ToString(),
                //            //Title
                //            Author = song.Author,
                //            UserLogin = song.UserLogin,
                //            Songs = new ObservableLinkedList<Song>()
                //        };
                //        NewAlbum.Title = song.Album;

                //        string sql = $"INSERT INTO Albums (Id,  Title,  Author,  UserLogin)" +
                //                                $" VALUES(@Id, @Title, @Author, @UserLogin);";

                //        await cnn.ExecuteAsync(sql, NewAlbum);

                        
                //        NewAlbum.Songs.AddLast(song);
                //        _musicStorage.Albums.Add(NewAlbum);
                       
                //    }
                //    else
                //    {
                //        var NAlbum = _musicStorage.Albums.FirstOrDefault(x => x.Title == "Неизвестный");
                //        if (NAlbum != null)
                //        {
                //            if (NAlbum.Songs == null)
                //                NAlbum.Songs = new ObservableLinkedList<Song>();
                //            NAlbum.Songs.AddLast(song);
                //        } 
                //        else
                //        {
                //            NAlbum = new Album()
                //            {
                //                Id = Guid.NewGuid().ToString(),
                //                Title = "Неизвестный",
                //                Author = song.Author,
                //                UserLogin = song.UserLogin,
                //                Songs = new ObservableLinkedList<Song>()
                //            };

                //            string sql = $"INSERT INTO Albums (Id,  Title,  Author,  UserLogin)" +
                //                             $" VALUES(@Id, @Title, @Author, @UserLogin);";

                //            await cnn.ExecuteAsync(sql, NAlbum);

                //            NAlbum.Songs.AddLast(song);
                //            _musicStorage.Albums.Add(NAlbum);
                //        }
                //    }

                    

                //}
            }
            catch (Exception)
            {

            }
        }

        private void SearchSongLocalFolder(Song song)
        {
            try
            {
                var FolderPath = GetSongFolderPath(song.LocalUrl);

                var LF = _musicStorage.LocalFolders.FirstOrDefault(x => x.LocalUrl == FolderPath);
                if (LF != null)
                {
                    if (LF.Songs == null)
                        LF.Songs = new ObservableLinkedList<Song>();
                    LF.Songs.AddLast(song);
                }
                //else
                //{
                //    var NewLF = new LocalFolder()
                //    {
                //        Id = Guid.NewGuid().ToString(),
                //        Title = FolderPath.Remove(0, FolderPath.IndexOf('\\') + 1),
                //        LocalUrl = FolderPath,
                //        UserLogin = song.UserLogin,
                //        OnDevice = true,
                //        OnServer = false,
                //        Songs = new ObservableLinkedList<Song>()
                //    };

                //    string sql = $"INSERT INTO LocalFolders (Id,  Title,  LocalUrl,  UserLogin,  OnServer,   OnDevice)" +
                //                                  $" VALUES(@Id, @Title, @LocalUrl, @UserLogin, @OnServer,  @OnDevice);";

                //    await cnn.ExecuteAsync(sql, NewLF);

                //    NewLF.Songs.AddLast(song);
                //    _musicStorage.LocalFolders.Add(NewLF);
                //}
               
            }
            catch (Exception)
            {
                
            }
        }

        private void SearchSongPlayLists(Song song)
        {
            try
            {
                if (song.PlayLists != null)
                    foreach (var playList in song.PlayLists)
                    {
                        var PL = _musicStorage.PlayLists.FirstOrDefault(x => x.Id == playList);
                        if (PL != null)
                        {
                            if (PL.Songs == null)
                                PL.Songs = new ObservableLinkedList<Song>();
                            //PL.Songs.AddLast(song);

                            try
                            {
                                //var q = Thread.CurrentThread.ManagedThreadId;
                                Application.Current.Dispatcher.Invoke(() => PL.Songs.AddLast(song));
                              
                                //Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                                //{
                                //    var q = Thread.CurrentThread.ManagedThreadId;
                                //    PL.Songs.AddLast(song);

                                //}));
                            }
                            catch(Exception ex)
                            {

                            }
                        }
                    }
            }
            catch (Exception)
            {

            }
        }

        #endregion


        #endregion


        #region Добавление/удаление треков

        public async Task<Song> AddSongAsync(string path, string optionalInfo = "")
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

                    if(!string.IsNullOrEmpty(optionalInfo))
                    {
                        switch(optionalInfo)
                        {
                            case "":

                                break;

                            default:

                                temp.PlayLists = new List<string>();
                                temp.PlayLists.Add(optionalInfo);

                                break;
                        }
                    }   

                    string sql = $"INSERT INTO Songs (Author,     Title,     Album,      Duration,  Year," +
                                                    $"PlayLists,  LocalURL,  UserLogin,  OnServer,  OnDevice,  OptionalInfo)" +
                                           $" VALUES(@Author,    @Title,    @Album,     @Duration, @Year," +
                                                   $"@PlayLists, @LocalURL, @UserLogin, @OnServer, @OnDevice, @OptionalInfo);";

                    await cnn.ExecuteAsync(sql, temp);
                    await Task.Run(() => SongIntegrating(temp));

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

        public async Task<bool> RemoveSongAsync(Song song)
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

        #endregion


        #region Добавление/удаление папок

        public async Task<LocalFolder> AddLocalFolderAsync(string path)
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
                    Songs = new ObservableLinkedList<Song>()
                };
                _musicStorage.LocalFolders.Add(temp);

                foreach (var item in tempMas)
                    await AddSongAsync(item);

                return temp;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<bool> RemoveLocalFolderAsync(LocalFolder localFolder)
        {
            try
            {
                foreach (Song song in localFolder.Songs)
                    await RemoveSongAsync(song);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion


        #region Добавление/удаление плейлистов

        public async Task<PlayList> AddPlayListAsync(string title, string description, string imageUri)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    if (string.IsNullOrEmpty(imageUri))
                        imageUri = @"C:\Users\ivans\source\repos\FierceStukCloud\FierceStukCloud.Core\Images\fsc_icon.png";

                    var NewPlayList = new PlayList()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = title,
                        Description = description,
                        ImageUri = imageUri,
                        CreationDate = DateTime.Now,
                        UserLogin = _user.Login,
                        OnDevice = true,
                        OnServer = false,
                        Songs = new ObservableLinkedList<Song>()
                    };


                    string sql = $"INSERT INTO PlayLists (Id,  Title,  Description,  ImageUri,  CreationDate,  UserLogin,  OnServer,   OnDevice)" +
                                               $" VALUES(@Id, @Title, @Description, @ImageUri, @CreationDate, @UserLogin, @OnServer,  @OnDevice);";

                    await cnn.ExecuteAsync(sql, NewPlayList);
                    _musicStorage.PlayLists.Add(NewPlayList);
                    return NewPlayList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> RemovePlayListAsync(PlayList playList)
        {
            try
            {

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Дополнительные методы

        private string LoadConnectionString(string id = "SQLite")
             => ConfigurationManager.ConnectionStrings[id].ConnectionString;

        private string GetSongFolderPath(string songPath)
            => songPath.Remove(songPath.LastIndexOf('\\'));

        #endregion

        public DataService(IMusicStorage musicStorage, User user)
        {
            SqlMapper.AddTypeHandler(typeof(List<string>), new DictTypeHandler());

            _musicStorage = musicStorage;
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