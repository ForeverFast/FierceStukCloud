using FierceStukCloud_Mobile.MVVM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FierceStukCloud_Mobile.ViewModels
{
    public abstract class BaseViewModel : OnPropertyChangedClass
    {
        public INavigation Navigation { get; set; }

        public ICommand BackCommand { protected set; get; }

        public virtual void InitiailizeCommands()
        {
            BackCommand = new Command(Back);
        }

        public virtual void Back()
        {
            Navigation.PopAsync();
        }
    }
}
