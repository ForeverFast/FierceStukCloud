using FierceStukCloud_NetCoreLib.Models;
using FierceStukCloud_NetCoreLib.Services;
using FierceStukCloud_NetCoreLib.Services.ImageAsyncS;
using System;
using System.Windows.Media.Imaging;

namespace FierceStukCloud_PC.MVVM.ViewModels
{
    public class MainWindowVM
    {


        public ImageAsyncCollection<ImageAsync<Song>> Songs { get; }

        //public ImageSource ImageDefault { get; set; }
        public RelayCommand LoadImagesCommand { get; }

        // загрузка изображений
        private void LoadImagesExecute(object parameter)
        {

            /// Загрузка из внешнего (Содержание) ресурса
            Songs.Add(new ImageAsync<Song>
            (
                @"Avicii - Waiting for Love (Original Mix).mp3",
                new Song() { Title = "WaitingForLove", Author = "Avichi" })
            );


            /// Загрузка из упакованного (Ресурс) ресурса
            Songs.Add
            (
                new ImageAsync<Song>(
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
                Songs = new ImageAsyncCollection<ImageAsync<Song>>();
            }
            else
            {
                // Контекст данных Времени Исполнения
                Songs = new ImageAsyncCollection<ImageAsync<Song>>(new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud_PC;component/Cd-disk-mp3.png")));
            }

            /// Общий Контекст Данных

        }
    }
}
