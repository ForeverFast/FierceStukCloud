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
            return (BitmapImage)uri;
        }


       
        //if (ImageSourceConverter.CanConvertFrom(uri.GetType()))
        //    return (ImageSource)ImageSourceConverter.ConvertFrom(uri);

        //throw new InvalidCastException($"В параметре {nameof(uri)} получен тип недопустиый для конвертации в ImageSource");
    }
}