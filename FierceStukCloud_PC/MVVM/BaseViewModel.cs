using System.Windows;
using System.Windows.Input;
using FierceStukCloud.Mvvm;
using FierceStukCloud_PC.MVVM;

namespace FierceStukCloud_NetCoreLib.ViewModels
{
    public class BaseViewModel : OnPropertyChangedClass
    {
        #region Управление окном

        public ICommand MinimizedWindowCommand { get; private set; }
        public ICommand CloseWindowCommand { get; private set; }
        public ICommand DragWindowCommand { get; private set; }

        public virtual void MinimizedWindowMethod(object parameter) => Application.Current.MainWindow.WindowState = WindowState.Minimized;
        public virtual void CloseWindowMethod(object parameter)
        {
            Application.Current.MainWindow.Close(); 
        }
        public virtual void DragWindowMethod(object parameter) => Application.Current.MainWindow.DragMove();

        #endregion

        #region Конструкторы и методы инициализации

        public virtual void InitiailizeCommands()
        {
            MinimizedWindowCommand = new RelayCommand(MinimizedWindowMethod, null);
            CloseWindowCommand = new RelayCommand(CloseWindowMethod, null);
            DragWindowCommand = new RelayCommand(DragWindowMethod, null);
        }

        #endregion
    }
}
