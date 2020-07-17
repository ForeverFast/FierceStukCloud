using FierceStukCloud_Mobile.Models;
using FierceStukCloud_NetStandardLib.Models;
using FierceStukCloud_NetStandardLib.Models.AbstractModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace FierceStukCloud_Mobile.ViewModels
{
    public class MusicPlayerVM : BaseViewModel
    {
        private MusicPlayerM model { get; }

        public ObservableCollection<BaseMusicObject> Songs { get; set; }


        private Song _selectedSong;
        public Song SelectedSong
        {
            get => _selectedSong;
            set
            {
                if (value != null)
                {
                   // value.Content.CurrentMusicContainer = SelectedMusicContainer != null ?
                     //                                     SelectedMusicContainer : new LocalFolder() { Songs = GetLocalFilesAsMC() };

                    model.SetCurrentSong(value);
                    SetProperty(ref _selectedSong, value);
                }
            }
        }

        #region События

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
                switch (e.PropertyName)
                {
                    case nameof(model.MP_MediaOpened):
                        //SelectedStyle = "PauseButton";

                        //SongTime = model.MP.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                        //SongTimeLineForSlider = 0;
                        //SongTimeForSlider = model.MP.NaturalDuration.TimeSpan.TotalSeconds;

                        //SongName = model.CurrentSong.Title;
                        //SongAuthor = model.CurrentSong.Author;
                        //SongBitmapImage = model.CurrentImage;
                        break;

                    case nameof(model.Timer_Tick):

                        //if (PosChanges == false)
                        //{
                        //    SongPos = model.MP.Position.ToString(@"mm\:ss");
                        //    SongTimeLineForSlider = model.MP.Position.TotalSeconds;
                        //}

                        break;

                    case nameof(model.Play):

                        //SelectedStyle = "PauseButton";

                        break;

                    case nameof(model.Pause):
                    case nameof(model.Stop):

                        //SelectedStyle = "PlayButton";

                        break;
                         
                    case "GettingSongsEvent":

                        foreach (var item in model.LocalFiles)
                        {
                            try
                            { 
                                Songs.Add(item);
                            }
                            catch(Exception ex)
                            {
                                int a = 1 + 1;
                            }
                        }
                        

                        break;
                }
            }
        }

        #endregion


        #region Конструкторы и методы инициализации

        public MusicPlayerVM()
        {
            model = new MusicPlayerM();
            model.PropertyChanged += Model_PropertyChanged;

            Songs = new ObservableCollection<BaseMusicObject>(model.GetListLocalFiles());
        }

        #endregion
    }
}
