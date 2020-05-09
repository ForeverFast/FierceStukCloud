using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace FierceStukCloud_NetCoreLib.Services.ImageAsyncS
{
    /// <summary>Реализация базового класса асинхронной загрузки
    /// изображения</summary>
    public class ImageAsync : ImageAsyncBase
    {
        public ImageAsync(Dispatcher dispatcher)
            :base(dispatcher)
        {}

        /// <summary>Экземпляр конвертера</summary>
        public static ImageSourceConverter ImageSourceConverter { get; }
            = new ImageSourceConverter();
      
        public override ImageSource ImageLoad(object uri)
        {
            TagLib.File file_TAG = TagLib.File.Create((string)uri);
            try
            {
                if (file_TAG.Tag.Pictures.Length >= 1)
                {
                    var bin = file_TAG.Tag.Pictures[0].Data.Data; // Конвертация в массив байтов

                    var bitmapImage = new BitmapImage();

                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = new MemoryStream(bin);
                    bitmapImage.EndInit();

                    //var kek = file_TAG.Tag.Pictures[0].Type;
                    //var image = System.Drawing.Image.FromStream(new MemoryStream(bin)); // Конвертация в Image WinForm

                    //var bitmapImage = new BitmapImage();
                    //bitmapImage.StreamSource =new MemoryStream(bin);
                    //using (var ms = new MemoryStream()) // Создание потока конвертации изображения в BitmapImage
                    //{
                    //    image.Save(ms, ImageFormat.Jpeg);
                    //    ms.Seek(0, SeekOrigin.Begin);
                    //    bitmapImage.BeginInit();
                    //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    //    bitmapImage.StreamSource = ms;
                    //    bitmapImage.EndInit();
                    //}

                    return bitmapImage;
                }
                else
                    return ImageDefault;
            }
            catch (Exception)
            {
                try
                {
                    if (file_TAG.Tag.Pictures.Length >= 1)
                    {
                        var bin = file_TAG.Tag.Pictures[0].Data.Data; // Конвертация в массив байтов

                        var bitmapImage = new BitmapImage();

                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = new MemoryStream(bin);
                        bitmapImage.EndInit();

                        //var image = System.Drawing.Image.FromStream(new MemoryStream(bin)); // Конвертация в Image WinForm
                        //var bitmapImage = new BitmapImage();
                        //using (var ms = new MemoryStream()) // Создание потока конвертации изображения в BitmapImage
                        //{
                        //    image.Save(ms, ImageFormat.Png);
                        //    ms.Seek(0, SeekOrigin.Begin);
                        //    bitmapImage.BeginInit();
                        //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        //    bitmapImage.StreamSource = ms;
                        //    bitmapImage.EndInit();
                        //}
                        return bitmapImage;
                    }
                    else
                        return ImageDefault;
                }
                catch (Exception)
                {
                    return ImageDefault;
                }
            }

            //return (BitmapImage)uri;
        }


       
        //if (ImageSourceConverter.CanConvertFrom(uri.GetType()))
        //    return (ImageSource)ImageSourceConverter.ConvertFrom(uri);

        //throw new InvalidCastException($"В параметре {nameof(uri)} получен тип недопустиый для конвертации в ImageSource");
    }
}