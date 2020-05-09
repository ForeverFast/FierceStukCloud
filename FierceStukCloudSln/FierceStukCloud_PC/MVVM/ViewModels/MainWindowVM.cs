using FierceStukCloud_NetCoreLib.Models;
using FierceStukCloud_NetCoreLib.Services;
using FierceStukCloud_NetCoreLib.Services.ImageAsyncS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FierceStukCloud_PC.MVVM.ViewModels
{
    public class MainWindowVM
    {
        public ObservableCollection<Song> Songs { get; set; }


        public ImageSource ImageDefault { get; set; }
        public RelayCommand LoadImagesCommand { get; }

        // загрузка изображений
        private void LoadImagesExecute(object parameter)
        {
            Songs.Add(new Song()
            {
                Title = "WaitingForLove",
                Author = "Avichi",
               
                Image = new ImageAsync()
                {
                    ImageDefault = ImageDefault,
                    // Если нижнюю строчку расскоменить - будете ловить исключение
                    //ImageUri = @"C:\Users\ivans\Desktop\c\Avicii - Waiting for Love (Original Mix).mp3"
                }
            });
            // тут синхронно будет грузить пикчу из файла на ПК
            Songs.Last().Image.ImageUri = GetImage(@"C:\Users\ivans\Desktop\c\Avicii - Waiting for Love (Original Mix).mp3");
        }

        private BitmapImage GetImage(string url)
        {
            TagLib.File file_TAG = TagLib.File.Create(url);
            try
            {
                if (file_TAG.Tag.Pictures.Length >= 1)
                {
                    var bin = (byte[])(file_TAG.Tag.Pictures[0].Data.Data); // Конвертация в массив байтов
                    var kek = file_TAG.Tag.Pictures[0].Type;
                    var image = System.Drawing.Image.FromStream(new MemoryStream(bin)); // Конвертация в Image WinForm
                    var bitmapImage = new BitmapImage();
                    using (var ms = new MemoryStream()) // Создание потока конвертации изображения в BitmapImage
                    {
                        image.Save(ms, ImageFormat.Jpeg);
                        ms.Seek(0, SeekOrigin.Begin);
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = ms;
                        bitmapImage.EndInit();
                    }
                    return bitmapImage;
                }
                else
                    return GetDefaultPic();
            }
            catch (Exception)
            {
                try
                {
                    if (file_TAG.Tag.Pictures.Length >= 1)
                    {
                        var bin = (byte[])(file_TAG.Tag.Pictures[0].Data.Data); // Конвертация в массив байтов

                        var image = System.Drawing.Image.FromStream(new MemoryStream(bin)); // Конвертация в Image WinForm
                        var bitmapImage = new BitmapImage();
                        using (var ms = new MemoryStream()) // Создание потока конвертации изображения в BitmapImage
                        {
                            image.Save(ms, ImageFormat.Png);
                            ms.Seek(0, SeekOrigin.Begin);
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = ms;
                            bitmapImage.EndInit();
                        }
                        return bitmapImage;
                    }
                    else
                        return GetDefaultPic();
                }
                catch (Exception)
                {
                    return GetDefaultPic();
                }
            }
        }

        private BitmapImage GetDefaultPic() => new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud_Lib;component/Resources/Em1.png"));


        public MainWindowVM()
        {
            Songs = new ObservableCollection<Song>();



            LoadImagesCommand = new RelayCommand(LoadImagesExecute, null);




            ImageDefault = new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud_NetCoreLib;component/Resources/Images/fsc_icon.png"));
        }
    }
}
