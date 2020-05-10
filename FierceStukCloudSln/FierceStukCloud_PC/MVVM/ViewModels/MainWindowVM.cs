using FierceStukCloud_NetCoreLib.Models;
using FierceStukCloud_NetCoreLib.Services;
using FierceStukCloud_NetCoreLib.Services.ImageAsyncS;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace FierceStukCloud_PC.MVVM.ViewModels
{
	public class MainWindowVM
	{

		public Dispatcher Dispatcher { get; }

		/// <summary>Конструктор с передачей Диспетчера</summary>
		/// <param name="dispatcher">Диспетчер UI потока</param>
		public MainWindowVM(Dispatcher dispatcher)
			: this(false)
		{
			Dispatcher = dispatcher;
			Songs = new ImageAsyncCollection<ImageAsync<Song>>
			(
				Dispatcher, 
				new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud_PC;component/Cd-disk-mp3.png"))
			);

		}

		//public ObservableCollection<Song> Songs { get; } = new ObservableCollection<Song>();

		public ImageAsyncCollection<ImageAsync<Song>> Songs { get; }

		//public ImageSource ImageDefault { get; set; }
		public RelayCommand LoadImagesCommand { get; }

		// загрузка изображений
		private void LoadImagesExecute(object parameter)
		{

			/// Загрузка из внешнего (Содержание) ресурса
			Songs.Add(new ImageAsync<Song>
			(
				Dispatcher,
				@"Avicii - Waiting for Love (Original Mix).mp3", 
				new Song() { Title = "WaitingForLove", Author = "Avichi" })
			);


			/// Загрузка из упакованного (Ресурс) ресурса
			Songs.Add
			(
				new ImageAsync<Song>(Dispatcher,
				new Uri("pack://application:,,,/FierceStukCloud_PC;component/AviciiPackaged.mp3"),
				new Song() { Title = "WaitingForLove (Packaged)", Author = "Avichi" })
			);

		}
		


		/// <summary>Конструктор Времени Разработки Дизайна</summary>
		public MainWindowVM() : this(true) { }

		public bool IsDesignMode { get; }

		/// <summary>Общий Конструктор</summary>
		/// <param name="isDesignMode"><see langword="true"/> если вызван во Время Разработки Дизайна</param>
		public MainWindowVM(bool isDesignMode)
		{
			IsDesignMode = isDesignMode;

			/// Общий Контекст Данных
			LoadImagesCommand = new RelayCommand(LoadImagesExecute, null);

			if (isDesignMode)
			{
				// Контекст данных Времени Разработки Дизайна
				Songs = new ImageAsyncCollection<ImageAsync<Song>>(Dispatcher);
			}
			else
			{
				// Контекст данных Времени Исполнения
			}

			/// Общий Контекст Данных

		}
	}
}
