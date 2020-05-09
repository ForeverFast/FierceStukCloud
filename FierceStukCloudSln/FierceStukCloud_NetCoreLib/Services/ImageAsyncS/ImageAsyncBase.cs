﻿using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace FierceStukCloud_NetCoreLib.Services.ImageAsyncS
{
	/// <summary>Базовый класс с ImageSource загружаемым
	/// ассинхронно после первого требования</summary>
	public abstract class ImageAsyncBase : OnPropertyChangedClass
	{
		#region Поля для хранения значений свойств
		private ImageSource _imageDefault;
		private bool _isImageLoaded;
		private object _imageUri;
		#endregion

		public Dispatcher Dispatcher { get; }

		protected ImageAsyncBase(Dispatcher dispatcher)
		{
			Dispatcher = dispatcher;
		}



		/// <summary>Изображение по умолчанию.
		/// Выводится пока не загружено основное изображение</summary>
		public ImageSource ImageDefault { get => _imageDefault; set => SetProperty(ref _imageDefault, value); }

		/// <summary>Флаг загружености основного изображения</summary>
		public bool IsImageLoaded { get => _isImageLoaded; private set => SetProperty(ref _isImageLoaded, value); }

		/// <summary>Uri основного изображения.
		/// Может быть не обязательно типа Uri.
		/// Но записанный тип должен правильно обрабытываться
		/// в методе ImageLoad определяемом в производном классе.</summary>
		public object ImageUri { get => _imageUri; set => SetProperty(ref _imageUri, value); }

		/// <summary>Основное изображение</summary>
		public ImageSource Image
		{
			get
			{
				if (IsImageLoaded)
					return UploadedImage;

				if (ImageUri != DownloadUri)
					ImageLoad();
				return ImageDefault;
			}
		}

		/// <summary>Загруженное изображение</summary>
		protected ImageSource UploadedImage { get; private set; }

		/// <summary>Загружаемый Uri</summary>
		protected object DownloadUri { get; private set; }

		/// <summary>Асинхронная загрузка изображения</summary>
		private void ImageLoad()
		{
			if (ImageUri == default)
				return;
			object ImageUriLoad = DownloadUri = ImageUri;
			ImageSource image = null;
			if (Dispatcher == null)
				image = ImageLoad(ImageUriLoad);
			else
				Dispatcher.Invoke(() => image = ImageLoad(ImageUriLoad));
			if (ImageUriLoad.Equals(ImageUri))
			{
				UploadedImage = image;
				IsImageLoaded = true;
			}
		}

		/// <summary>Синхронная загрузка изображения заданого Uri</summary>
		/// <param name="uri">Uri изображения</param>
		/// <returns>ImageSource с загруженным изображением</returns>
		public abstract ImageSource ImageLoad(object uri);

		protected override void PropertyNewValue<T>(ref T fieldProperty, T newValue, string propertyName)
		{
			base.PropertyNewValue(ref fieldProperty, newValue, propertyName);

			if (propertyName == nameof(ImageDefault))
			{
				if (!IsImageLoaded)
					OnPropertyChanged(nameof(Image));
			}
			else if (propertyName == nameof(IsImageLoaded))
			{
				OnPropertyChanged(nameof(Image));
			}
			else if (propertyName == nameof(ImageUri))
			{
				IsImageLoaded = false;
			}
		}
	}
}
