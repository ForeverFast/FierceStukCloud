using FierceStukCloud_Mobile.Models;
using FierceStukCloud_Mobile.MVVM.Views;
using FierceStukCloud_Mobile.ViewModels;
using FierceStukCloud_NetStandardLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace FierceStukCloud_Mobile.MVVM.ViewModels.AbstractVM
{
    public class BaseListSongsVM : BaseViewModel
    {
        public MusicPlayerM model { get; set; }


        public ObservableCollection<Song> Songs { get; set; }


        private Song _selectedSong;

        public Song SelectedSong
        {
            get => _selectedSong;
            set
            {
                if (value != null)
                {
                    //value.CurrentMusicContainer = SelectedMusicContainer != null ?
                    //                                      SelectedMusicContainer : new LocalFolder() { Songs = GetLocalFilesAsMC() };

                    model.SetCurrentSong(value);
                    SetProperty(ref _selectedSong, value);
                }
            }
        }

        #region События

        public virtual void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
                switch (e.PropertyName)
                {
                    case nameof(model.MP_MediaOpened):

                        Navigation.PushAsync(new PlayerPage(model));

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
                }
            }
        }

        #endregion

    }
}
