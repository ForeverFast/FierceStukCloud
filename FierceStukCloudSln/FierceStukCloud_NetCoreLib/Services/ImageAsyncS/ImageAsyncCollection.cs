using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Threading;

namespace FierceStukCloud_NetCoreLib.Services.ImageAsyncS
{
	public class ImageAsyncCollection : ObservableCollection<ImageAsyncBase>
	{
		private ImageSource _imageDefault;
		private Dispatcher _dispatcher;

		/// <summary>Изображение по умолчанию.
		/// Выводится пока не загружено основное изображение</summary>
		public ImageSource ImageDefault
		{
			get => _imageDefault;
			set
			{
				if (_imageDefault == value) return;
				_imageDefault = value;
				OnPropertyChanged(ImageDefaultArgs);
			}
		}
		private static readonly PropertyChangedEventArgs ImageDefaultArgs
			= new PropertyChangedEventArgs(nameof(ImageDefault));
		private static readonly PropertyChangedEventArgs DispatcherArgs
			= new PropertyChangedEventArgs(nameof(Dispatcher));

		public Dispatcher Dispatcher
		{
			get => _dispatcher;
			internal set
			{
				if (_dispatcher == value) return;
				_dispatcher = value;
				OnPropertyChanged(DispatcherArgs);
			}
		}

		public ImageAsyncCollection(Dispatcher dispatcher)
		{
			PropertyChanged += ImageAsyncCollection_PropertyChanged;
			Dispatcher = dispatcher;
		}

		public ImageAsyncCollection(Dispatcher dispatcher, ImageSource imageDefault)
			: this(dispatcher)
		{
			ImageDefault = imageDefault;
		}

		protected override void InsertItem(int index, ImageAsyncBase item)
		{
			item.Dispatcher = Dispatcher;
			item.ImageDefault = ImageDefault;
			base.InsertItem(index, item);
		}

		private void ImageAsyncCollection_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == nameof(ImageDefault))
				foreach (ImageAsyncBase image in this)
					image.ImageDefault = ImageDefault;
			if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == nameof(Dispatcher))
				foreach (ImageAsyncBase image in this)
					image.ImageDefault = ImageDefault;
		}
	}

}
