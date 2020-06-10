using System.Windows;
using FierceStukCloud_NetCoreLib.Services;

namespace FierceStukCloud_NetCoreLib.ViewModels
{
    public class BaseViewModel : OnPropertyChangedClass
    {
        #region Управление окном

        public RelayCommand MinimizedWindowCommand { get; private set; }
        public RelayCommand CloseWindowCommand { get; private set; }
        public RelayCommand DragWindowCommand { get; private set; }

        public void MinimizedWindowMethod(object parameter) => Application.Current.MainWindow.WindowState = WindowState.Minimized;
        public void CloseWindowMethod(object parameter) => Application.Current.MainWindow.Close();
        public void DragWindowMethod(object parameter) => Application.Current.MainWindow.DragMove();

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
