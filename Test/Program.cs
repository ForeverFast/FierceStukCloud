using Dapper;
using FierceStukCloud_NetStandardLib.Models;
using FierceStukCloud_NetStandardLib.Models.AbstractModels;
using FierceStukCloud_NetStandardLib.Models.MusicContainers;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using TagLib.Ape;
using static Dapper.SqlMapper;

namespace Test
{
    

    class Program
    {

        static void Main(string[] args)
        {
            string str = "Data Source=fscLocalDB.db;Version=3;";

            ObservableCollection<BaseMusicObject> ListBMO = new ObservableCollection<BaseMusicObject>();
            ListBMO.Add(new Song() { Title = "rofl" });
            ListBMO.Add(new Song() { Title = "kek" });

            ObservableCollection<BaseMusicObject> ObservableCollectionBMO = ListBMO;

            ObservableCollectionBMO.Add(new Song() { Title = "lol" });

            var json1 = System.Text.Json.JsonSerializer.Serialize<ObservableCollection<BaseMusicObject>>(ObservableCollectionBMO);






            //using (IDbConnection cnn = new SQLiteConnection(str))
            //{
            //    SqlMapper.AddTypeHandler(typeof(List<string>), new DictTypeHandler());
            //    var outout = cnn.Query<Song>("SELECT * FROM Songs");
            //    var temp = outout.ToList();



            //    cnn.Execute("Update Songs SET ")

            //}

            //List<string> ts = new List<string>();
            //  ts.Add("kek");
            //  ts.Add("lol");

            //  string s = JsonConvert.SerializeObject(ts);



        }


    }

    //internal sealed class DictionaryKeyValueConverter : JsonConverterFactory
    //{
    //    public override bool CanConvert(Type typeToConvert)
    //    {
    //        if (!typeToConvert.IsGenericType)
    //        {
    //            return false;
    //        }

    //        if (typeToConvert.GetGenericTypeDefinition() != typeof(Dictionary<,>))
    //        {
    //            return false;
    //        }

    //        // Don't change semantics of Dictionary<string, TValue> which uses JSON properties (not array of KeyValuePairs).
    //        Type keyType = typeToConvert.GetGenericArguments()[0];
    //        if (keyType == typeof(string))
    //        {
    //            return false;
    //        }

    //        return true;
    //    }

    //    public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
    //    {
    //        Type keyType = type.GetGenericArguments()[0];
    //        Type valueType = type.GetGenericArguments()[1];

    //        JsonConverter converter = (JsonConverter)Activator.CreateInstance(
    //            typeof(DictionaryKeyValueConverterInner<,>).MakeGenericType(new Type[] { keyType, valueType }),
    //            BindingFlags.Instance | BindingFlags.Public,
    //            binder: null,
    //            args: new object[] { options },
    //            culture: null);

    //        return converter;
    //    }

    //    private class DictionaryKeyValueConverterInner<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>>
    //    {
    //        private readonly JsonConverter<KeyValuePair<TKey, TValue>> _converter;

    //        public DictionaryKeyValueConverterInner(JsonSerializerOptions options)
    //        {
    //            _converter = (JsonConverter<KeyValuePair<TKey, TValue>>)options.GetConverter(typeof(KeyValuePair<TKey, TValue>));

    //            // KeyValuePair<> converter is built-in.
    //            Debug.Assert(_converter != null);
    //        }

    //        public override Dictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //        {
    //            if (reader.TokenType != JsonTokenType.StartArray)
    //            {
    //                throw new JsonException();
    //            }

    //            Dictionary<TKey, TValue> value = new Dictionary<TKey, TValue>();

    //            while (reader.Read())
    //            {
    //                if (reader.TokenType == JsonTokenType.EndArray)
    //                {
    //                    return value;
    //                }

    //                KeyValuePair<TKey, TValue> kv = _converter.Read(ref reader, typeToConvert, options);
    //                value.Add(kv.Key, kv.Value);
    //            }

    //            throw new JsonException();
    //        }

    //        public override void Write(Utf8JsonWriter writer, Dictionary<TKey, TValue> value, JsonSerializerOptions options)
    //        {
    //            writer.WriteStartArray();

    //            foreach (KeyValuePair<TKey, TValue> kvp in value)
    //            {
    //                _converter.Write(writer, kvp, options);
    //            }

    //            writer.WriteEndArray();
    //        }
    //    }
    //}



}


/*
  try
            {
 Console.WriteLine(kek.kekw.ToString());

            var localSongs = new List<Song>();
            var albums = new List<Album>();
            var localFolders = new List<LocalFolder>();
            var playLists = new List<PlayList>();


                List<Song> songs = new List<Song>();
                Song song1 = new Song();
                song1.Author = "trr";
                song1.Album = "kek";
                song1.LocalUrl = @"D:\Музыка\Zara Larsson - Never forget you (Price & Takis Remix) _ Monsterbrony.mp3";
                song1.PlayLists.Add("lol");
                songs.Add(song1);

                Song song2 = new Song();
                song2.Author = "trr";
                song2.Album = "kek";
                song2.LocalUrl = @"D:\Музыка\Zara Larsson - Never forget you (Price & Takis Remix) _ Monsterbrony.mp3";
                song2.PlayLists.Add("rofl");
                songs.Add(song2);

                Song song3 = new Song();
                song3.Author = "trr2";
                song3.Album = "brr";
                song3.LocalUrl = @"D:\Музыка\Zara Larsson - Never forget you (Price & Takis Remix) _ Monsterbrony.mp3";
                song3.PlayLists.Add("rofl");
                songs.Add(song3);

                foreach (var item in songs)
                {
                    if (item.OptionalInfo == "LF")
                    {
                        //item.LocalID = localSongs.Count;
                        localSongs.Add(item);
                        continue;
                    }

                    #region Поиск альбома

                    var Album = albums.Find(x => x.Title.ToLower() == item.Album.ToLower() &&
                                                 x.Author.ToLower() == item.Author.ToLower());
                    if (Album != null)
                    {
                        //item.LocalID = Album.Songs.Count;
                        item.LocalId.Add(new KeyValuePair<MusicContainer, int>(Album,Album.Songs.Count));
                        Album.Songs.Add(item);
                    }
                    else
                    {
                        var NewAlbum = new Album()
                        {
                            Title = item.Album,
                            Author = item.Author,
                            UserLogin = ""
                        };

                        // item.LocalID = NewAlbum.Songs.Count;
                        item.LocalId.Add(new KeyValuePair<MusicContainer, int>(NewAlbum, NewAlbum.Songs.Count));
                        NewAlbum.Songs.Add(item);
                        albums.Add(NewAlbum);
                    }

                    #endregion


                    #region Поиск папки

                    var itemPath = item.LocalUrl;
                    var path = itemPath.Remove(itemPath.LastIndexOf('\\'));

                    var LF = localFolders.Find(x => x.LocalUrl == path);
                    if (LF != null)
                    {
                        //item.LocalID = LF.Songs.Count + 1;
                        item.LocalId.Add(new KeyValuePair<MusicContainer, int>(LF, LF.Songs.Count));
                        LF.Songs.Add(item);
                    }
                    else
                    {
                        var NewLF = new LocalFolder()
                        {
                            Title = itemPath.Remove(0, path.Length + 1),
                            LocalUrl = path,
                            UserLogin = ""
                        };

                        //item.LocalID = NewLF.Songs.Count + 1;
                        item.LocalId.Add(new KeyValuePair<MusicContainer, int>(NewLF, NewLF.Songs.Count));
                        NewLF.Songs.Add(item);
                        localFolders.Add(NewLF);
                    }

                    #endregion


                    #region Поиск плейлистов

                    foreach (var playList in item.PlayLists)
                    {
                        var PL = playLists.Find(x => x.Title == playList);
                        if (PL != null)
                        {
                            item.LocalId.Add(new KeyValuePair<MusicContainer, int>(PL, PL.Songs.Count));
                            PL.Songs.Add(item);
                        }
                        else
                        {
                            var NewPL = new PlayList()
                            {
                                Title = playList,
                                UserLogin = ""
                            };

                            item.LocalId.Add(new KeyValuePair<MusicContainer, int>(NewPL, NewPL.Songs.Count));
                            NewPL.Songs.Add(item);
                            playLists.Add(NewPL);
                        }
                    }


                    #endregion


                }








                //var options = new JsonSerializerOptions();
                //options.Converters.Add(new DictionaryKeyValueConverter());
                //options.MaxDepth = 5;
                ////options.Han

                //string json = JsonSerializer.Serialize(song1, options);
                //var s = JsonSerializer.Deserialize<Song>(json, options);

                //JsonSerializerSettings settings = new JsonSerializerSettings();
                //settings.ContractResolver = new DictionaryAsArrayResolver();

                //string str = JsonConvert.SerializeObject(song1, Formatting.Indented);

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                settings.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
                settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
                settings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;

                string str = JsonConvert.SerializeObject(localFolders, settings);
                var song = JsonConvert.DeserializeObject<List<LocalFolder>>(str, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
                });

                str = JsonConvert.SerializeObject(song1.LocalId, settings);
              
                var t = JsonConvert.DeserializeObject<List<KeyValuePair<MusicContainer, int>>>(str, settings);
                var q = t[0].Key.Songs[0].Title;

                //var json = new JavaScriptSerializer().Serialize(song1.LocalId.ToDictionary(item => item.Key.ToString(), item => item.Value.ToString()));
                //Dictionary<Album, int> t = new JavaScriptSerializer().Deserialize<Dictionary<Album, int>>(json); // JsonConvert.DeserializeObject<Dictionary<MusicContainer, int>>(json);
            }
            catch (Exception ex) 
            {

            }
            finally
            {

            }
 */
