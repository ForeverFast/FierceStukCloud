using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FierceStukCloud_NetCoreLib.Services.ImageAsyncS
{
    /// <summary>Реализация базового класса асинхронной загрузки
    /// изображения</summary>
    public class ImageAsync : ImageAsyncBase
    {
        /// <summary>Экземпляр конвертера</summary>
        public static ImageSourceConverter ImageSourceConverter { get; }
            = new ImageSourceConverter();
      
        public override ImageSource ImageLoad(object uri)
        {
            try
            {
                TagLib.File file_TAG = TagLib.File.Create((string)uri);
                var bin = file_TAG.Tag.Pictures[0].Data.Data; // Конвертация в массив байтов

                var bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = new MemoryStream(bin);
                bitmapImage.EndInit();

                return bitmapImage;
            }
            catch (Exception)
            {
                return GetDefaultPic();
            }
        }

        private BitmapImage GetDefaultPic() => 
            new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud_NetCoreLib;component/Resources/Images/fsc_icon.png"));

        //if (ImageSourceConverter.CanConvertFrom(uri.GetType()))
        //    return (ImageSource)ImageSourceConverter.ConvertFrom(uri);

        //throw new InvalidCastException($"В параметре {nameof(uri)} получен тип недопустиый для конвертации в ImageSource");
    }
}