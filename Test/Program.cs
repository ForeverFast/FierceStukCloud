using Dapper;

using FierceStukCloud.Core.Services;
using FierceStukCloud.Pc.Mvvm;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using TestNetStandardLib;
using static Dapper.SqlMapper;

namespace Test
{
    //class class1
    //{
    //    public void Go()
    //    {

    //    }
    //}

    //static class dopclass1
    //{
    //    public static void GoFast(this class1 objectClass)
    //    {

    //    }
    //}



    //public class t1 : IDisposable
    //{
    //    public int q;


    //    ~t1()
    //    {
    //        q = 0;
    //    }

    //    public void Dispose()
    //    {

    //    }
    //    //public ObservableCollection<Song> Songs { get; set; }
    //}

    //public class t2
    //{

    //   // public ObservableCollection<Song> Songs { get; set; }
    //}

    public abstract class TRObject
    {
        public string Node { get; set; }
        public string Text { get; set; }
        public bool IsOptional { get; set; }
    }


    public class TranslationResponse
    {
        public string Title { get; set; }
        public List<TitleMarkup> TitleMarkup { get; set; }
        public string Dictionary { get; set; }
        public string ArticleId { get; set; }

        public List<BodyItem> Body { get; set; }

        public List<TRObject> SearchObject(Func<TRObject, bool> predicate)
        {
            var result = new List<TRObject>();

            if (Body != null)
            {
                result.AddRange(Body);
                foreach (var item in Body)
                    result.AddRange(item.SearchObject(predicate));
            }

            return result.Where(predicate).ToList();
        }
    }

    public class TitleMarkup : TRObject
    {
        public bool IsItalics { get; set; }
        public bool IsAccent { get; set; }
    }

    public class BodyItem : TRObject
    {
        public object Type { get; set; }

        public List<MarkupItem> Markup { get; set; }

        public List<BodyItem> Items { get; set; }



        public List<TRObject> SearchObject(Func<TRObject, bool> predicate)
        {
            var result = new List<TRObject>();

            if (Markup != null)
            {
                result.AddRange(Markup);
                foreach (var item in Markup)
                    result.AddRange(item.SearchObject(predicate));
            }

            if (Items != null)
            {
                result.AddRange(Items);
                foreach (var item in Items)
                    result.AddRange(item.SearchObject(predicate));
            }

            return result.Where(predicate).ToList();
        }
    }

    public class MarkupItem : TRObject
    {
        public string Dictionary { get; set; }
        public string ArticleId { get; set; }

        public List<MarkupItem> Markup { get; set; }
        public List<BodyItem> Items { get; set; }

        public bool IsItalics { get; set; }
        public bool IsAccent { get; set; }

        public string FullText { get; set; }
        public string FileName { get; set; }

        public List<TRObject> SearchObject(Func<TRObject, bool> predicate)
        {
            var result = new List<TRObject>();

            if (Markup != null)
            {
                result.AddRange(Markup);
                foreach (var item in Markup)
                    result.AddRange(item.SearchObject(predicate));
            }

            if (Items != null)
            {
                result.AddRange(Items);
                foreach (var item in Items)
                    result.AddRange(item.SearchObject(predicate));
            }

            return result.Where(predicate).ToList();
        }
    }

    class Program
    {

        //private static void CommandManager_RequerySuggested(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //public static t1 temp { get => new t1() { q = new Random().Next(0, 10) }; }

        //public static ICommand Command { get => new RelayCommand((o) => {
        //    Console.WriteLine(new Random().Next(0, 10));
        //}); }

        static void Main(string[] args)
        {
            var client = new RestClient("https://developers.lingvolive.com/api/v1/Translation?text=floor&srcLang=1033&dstLang=1049");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer ZXlKaGJHY2lPaUpJVXpJMU5pSXNJblI1Y0NJNklrcFhWQ0o5LmV5SmxlSEFpT2pFMk1EQTJOelUzTWpNc0lrMXZaR1ZzSWpwN0lrTm9ZWEpoWTNSbGNuTlFaWEpFWVhraU9qVXdNREF3TENKVmMyVnlTV1FpT2pNNU5qSXNJbFZ1YVhGMVpVbGtJam9pTURZME5EUTFOVEV0T0RjeFpTMDBNamd4TFdJeFpEY3RPVGM0TjJZNFl6TTRPVEk0SW4xOS5YZXdFVUF6a19OV1ZMMDc1MUhDcUZ2Y0lpcXl6b01qUlV2dHpmU2VVS1U4");
            IRestResponse response = client.Execute(request);

            var json = response.Content;

            //var temp = JContainer.Parse(json);

            var t = System.Text.Json.JsonSerializer.Deserialize<List<TranslationResponse>>(json);

            foreach (var item in t)
            {
                var q = item.SearchObject(x => x.Node == "Text");
            }


         
                


            //var q = new System.Text.Json.JsonSerializer();
            //var updates = JObject.Parse(response.Content).SelectToken("Mackup");
            //var temp = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);


            // Console.WriteLine(response.Content);


            


            Console.WriteLine("End");

            //t1 t1 = new t1();
            //t1.q = 2;

            //t1 = new t1();





            // TR[0].body[1].items[2].Markup[1].items[0].Markup[0].Markup[0]

            //Command.Execute(new object());
            //Command.Execute(new object());

            //CommandManager.RequerySuggested += CommandManager_RequerySuggested;

            //var temp1 = temp;
            //var temp2 = temp;

            //var songs = new ReadOnlyCollection<Song>(new List<Song> { new Song { Title = "kek" } });
            //var q = songs.FirstOrDefault(x => x.Title == "kek");
            //q = new Song();

            //ObservableCollection<Song> songs = new ObservableCollection<Song>();
            //songs.Add(new Song() { Title = "1" });

            //t1 t1 = new t1();
            //t1.Songs = songs;

            //t1.Songs.Add(new Song() { Title = "2" });

            //t2 t2 = new t2();
            //t2.Songs = songs;

            //t2.Songs.Add(new Song() { Title = "2" });

            //var q = songs.FirstOrDefault(x => x.Title == "1");
            //q.Title = "4";


            //double[] t1 = new double[10];
            //double[] t2 = new double[3];

            //t2[0] = 1;
            //t2[1] = 2;
            //t2[2] = 3;

            //Array.Copy(t2, t1, 3);

            //int q = 0;



            //var cc = "4556364607935616";

            //int index = 0;
            //while (index < cc.Length - 4)
            //{
            //    cc.Remove(index, 1);
            //    cc.Insert(index++, "#");
            //}



            //Dictionary<string, string> morseD = new Dictionary<string, string>();
            //morseD.Add(".-", "A");
            //morseD.Add("-...", "B");
            //morseD.Add("-.-.", "C");
            //morseD.Add("-..", "D");
            //morseD.Add(".", "E");
            //morseD.Add("..-.", "F");
            //morseD.Add("--.", "G");
            //morseD.Add("....", "H");
            //morseD.Add("..", "I");
            //morseD.Add(".---", "J");
            //morseD.Add("-.-", "K");
            //morseD.Add(".-..", "L");
            //morseD.Add("--", "M");
            //morseD.Add("-.", "N");
            //morseD.Add("---", "O");
            //morseD.Add(".--.", "P");
            //morseD.Add("--.-", "Q");
            //morseD.Add(".-.", "R");
            //morseD.Add("...", "S");
            //morseD.Add("-", "T");
            //morseD.Add("..-", "U");
            //morseD.Add("...-", "V");
            //morseD.Add(".--", "W");
            //morseD.Add("-..-", "X");
            //morseD.Add("-.--", "Y");
            //morseD.Add("--..", "Z");

            //morseD.Add("...---...", "SOS");
            //morseD.Add("-.-.--", "!");
            //morseD.Add(".-.-.-", ".");
            //morseD.Add("", " ");

            //string input = "      ...---... -.-.--   - .... .   --.- ..- .. -.-. -.-   -... .-. --- .-- -.   ..-. --- -..-   .--- ..- -- .--. ...   --- ...- . .-.   - .... .   .-.. .- --.. -.--   -.. --- --. .-.-.-  ";
            //string temp = "";
            //string[] mass = input.Split(' ');

            //int i = 0;
            //int k = mass.Length - 1;

            //while (true)
            //{
            //    if (mass[i] != "")
            //        break;
            //    i++;
            //}

            //while(true)
            //{
            //    if (mass[k] != "")
            //        break;
            //    k--;
            //}




            //for (int j = i; j <= k; j++)
            //{
            //    if (j != i)
            //        if (temp[temp.Length - 1] == ' ')
            //            j++;
            //    //var q = mass[j];
            //    temp += morseD.GetValueOrDefault(mass[j]);

            //}
            //Console.WriteLine(" ");

            //var path = @"D:\temp2";

            //string[] tempMas = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);

            //if (tempMas == null)
            //{
            //    Console.WriteLine("пусто");
            //}

            //string str = "lel";
            //str.Length




            //var t = Guid.NewGuid().ToString();
            //Console.WriteLine(t);






            //string str = "Data Source=fscLocalDB.db;Version=3;";

            //IList<BaseMusicObject> ListBMO = new ObservableCollection<BaseMusicObject>();
            //ListBMO.Add(new Song() { Title = "rofl" });
            //ListBMO.Add(new Song() { Title = "kek" });

            //ObservableCollection<BaseMusicObject> ObservableCollectionBMO = ListBMO as ObservableCollection<BaseMusicObject>;

            //ObservableCollectionBMO.Add(new Song() { Title = "lol" });

            //var json1 = System.Text.Json.JsonSerializer.Serialize<ObservableCollection<BaseMusicObject>>(ObservableCollectionBMO);






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
