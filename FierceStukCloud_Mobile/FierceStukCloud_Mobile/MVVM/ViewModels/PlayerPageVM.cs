using FierceStukCloud_Mobile.Models;
using FierceStukCloud_Mobile.MVVM.ViewModels.AbstractVM;
using FierceStukCloud_Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FierceStukCloud_Mobile.MVVM.ViewModels
{
    public class PlayerPageVM : BaseViewModel
    {
        public MusicPlayerM model;

        #region Отображение информации о текущем треке

        #region Поля
        private Image _songImage;
        private string _songName;
        private string _SongAuthor;

        private string _songTime = "99:99";
        private string _songPos = "00:00";
        private double _songTimeForSlider;
        private double _songTimeLineForSlider;

        private double _songVolumeForSlider;

        private int _selectedSongOnLB;

        private string _selectedStyle;
        #endregion

        public Image SongImage { get => _songImage; set => SetProperty(ref _songImage, value); }

        public string SongName { get => _songName; set => SetProperty(ref _songName, value); }

        public string SongAuthor { get => _SongAuthor; set => SetProperty(ref _SongAuthor, value); }

        /// <summary>Текущее время трека</summary>
        public string SongPos { get => _songPos; set => SetProperty(ref _songPos, value); }

        /// <summary>Общее время песни </summary>
        public string SongTime { get => _songTime; set => SetProperty(ref _songTime, value); }

        /// <summary>Общее время песни на слайдере </summary>
        public double SongTimeForSlider { get => _songTimeForSlider; set => SetProperty(ref _songTimeForSlider, value); }

        /// <summary>Позиция указателя на слайдере </summary>
        public double SongTimeLineForSlider { get => _songTimeLineForSlider; set => SetProperty(ref _songTimeLineForSlider, value); }

        public double SongVolumeForSlider { get => _songVolumeForSlider; set { /*model.MP.Volume = value;*/ SetProperty(ref _songVolumeForSlider, value); } }


        public int SelectedSongOnLB { get => _selectedSongOnLB; set => SetProperty(ref _selectedSongOnLB, value); }


        public string SelectedStyle { get => _selectedStyle; set => SetProperty(ref _selectedStyle, value); }

        #endregion

        #region Кнопки управления плеера

        public ICommand PrevSongCommand { get; private set; }
        public ICommand NextSongCommand { get; private set; }
        public ICommand PlaySongCommand { get; private set; }
        public ICommand PauseSongCommand { get; private set; }
        public ICommand StopSongCommand { get; private set; }

        private void PrevSongExecute() => model.PrevSong();
        private void NextSongExecute() => model.NextSong();
        private void PlaySongExecute() => model.Play();
        private void PauseSongExecute() => model.Pause();
        private void StopSongExecute() => model.Stop();

        #endregion
         
        #region Конструкторы и методы инициализациия

        public PlayerPageVM(MusicPlayerM model)
        {
            this.model = model;

            InitiailizeCommands();

            SelectedStyle = "PauseButton";

            SongTime = model.CurrentSong.Duration;
            SongTimeLineForSlider = 0;
            SongTimeForSlider = TimeSpan.Parse(model.CurrentSong.Duration).TotalSeconds;

            SongName = model.CurrentSong.Title;
            SongAuthor = model.CurrentSong.Author;

        }

        public override void InitiailizeCommands()
        {
            base.InitiailizeCommands();

            PrevSongCommand =  new Command(PrevSongExecute);
            NextSongCommand =  new Command(NextSongExecute);
            PlaySongCommand =  new Command(PlaySongExecute);
            PauseSongCommand = new Command(PauseSongExecute);
            StopSongCommand =  new Command(StopSongExecute);
        }

        #endregion
    }
}
