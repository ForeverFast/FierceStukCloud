using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Threading;

namespace FierceStukCloud_NetCoreLib.Services.ImageAsyncS
{
	/// <summary>Реализация базового класса асинхронной загрузки
	/// изображения с Контекстом данных</summary>
	public partial class ImageAsync<T> : ImageAsyncBase<T>
	{
		public ImageAsync(Dispatcher dispatcher)
			: base(dispatcher)
		{ }
		public ImageAsync(Dispatcher dispatcher, object uri)
			: this(dispatcher)
		{
			ImageUri = uri;
		}
		public ImageAsync(Dispatcher dispatcher, object uri, T content)
			: this(dispatcher, uri)
		{
			Content = content;
		}
		public ImageAsync()
			: base(null)
		{ }
		public ImageAsync(object uri)
			: this()
		{
			ImageUri = uri;
		}
		public ImageAsync(object uri, T content)
			: this(uri)
		{
			Content = content;
		}

		/// <summary>Конструктор BitmapImage.
		/// Должен выполняться в Диспетчере основногопотока!</summary>
		/// <param name="stream">Поток с источником изображения.</param>
		/// <returns>Новый экземпляр BitmapImage с загруженным изображением</returns>
		private BitmapImage ImageLoadDispatcher(Stream stream)
		{
			var bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
			bitmapImage.StreamSource = stream;
			bitmapImage.EndInit();
			return bitmapImage;
		}
		/// <summary>Делегат метода создания BitmapImage.
		/// Без делегата не возможно вызвать перегрузку Dispatcher.Invoke,
		/// возвращающую результат по переданому параметру</summary>
		/// <param name="stream">Поток с изображением</param>
		/// <returns>Новый экземпляр BitmapImage с загруженным изображением</returns>
		private delegate BitmapImage ImageLoadDispatcherHandler(Stream stream);

		public override ImageSource ImageLoad(object uri, Dispatcher dispatcher)
		{
			if (uri == null)
				return ImageDefault;

			/// Эмуляция задержки
			Thread.Sleep(5000);

			try
			{
				Uri _uri;
				if (uri is string str)
					_uri = new Uri(str);
				else
					_uri = (Uri)uri;

				StreamResourceInfo res = Application.GetResourceStream(_uri);
				TagLibFile resStream = new TagLibFile(_uri.LocalPath, res.Stream, null);
				TagLib.File file_TAG = TagLib.File.Create(resStream);
				if (file_TAG.Tag.Pictures.Length < 1)
					return ImageDefault;

				using var stream = new MemoryStream(file_TAG.Tag.Pictures[0].Data.Data);
				return (ImageSource)dispatcher.Invoke((ImageLoadDispatcherHandler)ImageLoadDispatcher, stream);

			}
			catch (Exception)
			{
				try
				{
					TagLib.File file_TAG = TagLib.File.Create((string)uri);
					if (file_TAG.Tag.Pictures.Length < 1)
						return ImageDefault;

					using var stream = new MemoryStream(file_TAG.Tag.Pictures[0].Data.Data);
					return (ImageSource)dispatcher.Invoke((ImageLoadDispatcherHandler)ImageLoadDispatcher, stream);


				}
				catch (Exception) { }
				return ImageDefault;
			}
		}

	}
}