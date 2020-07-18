using FierceStukCloud_Mobile.Models;
using FierceStukCloud_Mobile.MVVM.ViewModels;
using FierceStukCloud_Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FierceStukCloud_Mobile.MVVM.Views
{
    [DesignTimeVisible(false)]
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class PlayerPage : ContentPage
    {
        public PlayerPage(MusicPlayerM model)
        {
            InitializeComponent();
            BindingContext = new PlayerPageVM(model);
        }
    }
}