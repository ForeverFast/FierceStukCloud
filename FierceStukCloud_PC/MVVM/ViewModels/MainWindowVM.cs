using FierceStukCloud_NetCoreLib.Models;
using FierceStukCloud_NetCoreLib.Services;
using FierceStukCloud_NetCoreLib.Services.ImageAsyncS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FierceStukCloud_PC.MVVM.ViewModels
{
    public class MainWindowVM
    {
        public ObservableCollection<Song> Songs { get; set; }


        public ImageSource ImageDefault { get; set; }
        public RelayCommand LoadImages { get; }

        // загрузка изображений
        private void LoadImagesExecute(object parameter)
        {
            Songs.Add(new Song()
            {
                Title = "WaitingForLove",
                Author = "Avichi",
                LocalURL = @"C:\Users\ivans\source\repos\FierceStukCloud\FierceStukCloud_PC\Avicii - Waiting for Love (Original Mix).mp3",
                Image = new ImageAsync()
                {
                    ImageDefault = ImageDefault,
                    ImageUri = @"C:\Users\ivans\source\repos\FierceStukCloud\FierceStukCloud_PC\Avicii - Waiting for Love (Original Mix).mp3"
                }
            });

        }

        public MainWindowVM()
        {
            Songs = new ObservableCollection<Song>();



            LoadImages = new RelayCommand(LoadImagesExecute, null);

            ImageDefault = new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud_NetCoreLib;component/Resources/Images/fsc_icon.png"));
        }
    }
}
