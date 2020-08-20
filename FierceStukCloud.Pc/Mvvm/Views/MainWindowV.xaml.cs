using FierceStukCloud.Pc.Mvvm.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FierceStukCloud.Pc.Mvvm.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindowV.xaml
    /// </summary>
    public partial class MainWindowV : Window
    {
        public MainWindowV(MainWindowVM mainWindowVM)
        {
            InitializeComponent();
            DataContext = mainWindowVM;
        }

        private void empListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = sender;
            var parent = ((ListBox)sender).Parent as UIElement;
            parent.RaiseEvent(eventArg);
        }
    }
}
