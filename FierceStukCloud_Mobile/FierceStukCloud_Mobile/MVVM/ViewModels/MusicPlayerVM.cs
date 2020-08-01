using FierceStukCloud_Mobile.Models;
using FierceStukCloud_Mobile.MVVM.ViewModels;
using FierceStukCloud_Mobile.MVVM.ViewModels.AbstractVM;
using FierceStukCloud_Mobile.MVVM.Views;
using FierceStukCloud_NetStandardLib.Extension;
using FierceStukCloud_NetStandardLib.Models;
using FierceStukCloud_NetStandardLib.Models.AbstractModels;
using FierceStukCloud_NetStandardLib.Models.MusicContainers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace FierceStukCloud_Mobile.ViewModels
{
    public class MusicPlayerVM : BaseListSongsVM
    {
        #region Основные свойства

        private MusicContainer _selectedMusicContainer;
       
        public MusicContainer SelectedMusicContainer { get => _selectedMusicContainer; set => SetProperty(ref _selectedMusicContainer, value); }


        #endregion

        #region ListBox Локальных файлов

        public ObservableCollection<BaseMusicObject> LocalFiles { get; set; }
        
        private BaseMusicObject _selectedBMO;

        public BaseMusicObject SelectedBMO
        {
            get => _selectedBMO;
            set
            {
                if (value is Song)
                {
                    _selectedBMO = value;
                    SelectedSong = value.ToSong();
                }
                else
                {
                    _selectedBMO = value;
                    SelectedMusicContainer = value.ToMC();
                    Navigation.PushAsync(new ListSongsV() { BindingContext = new ListSongsVM(this.SelectedMusicContainer) { model = this.model } });
                }
            }
        }

        private List<Song> GetLocalFilesAsMC() => (from temp in LocalFiles where temp is Song select temp) as List<Song>;


        #endregion

        #region События

        public override void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.Model_PropertyChanged(sender, e);
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
                switch (e.PropertyName)
                {
                    case "UpdateInfoFromPC":

                        foreach (var item in model.LocalFiles)
                        {
                            try
                            {
                                LocalFiles.Add(item);
                            }
                            catch (Exception)
                            {

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

            LocalFiles = new ObservableCollection<BaseMusicObject>(model.GetListLocalFiles());
            Songs = new ObservableCollection<Song>();
        }

        #endregion
    }
}
