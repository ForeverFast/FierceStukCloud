using FierceStukCloud.Pc.Mvvm.ViewModels;
using System.Windows;

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
    }
}
