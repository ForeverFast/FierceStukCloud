using FierceStukCloud_NetCoreLib.ViewModels;
using FierceStukCloud_PC.MVVM.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FierceStukCloud_PC.MVVM.Views.TestView
{
    /// <summary>
    /// Логика взаимодействия для Test1.xaml
    /// </summary>
    public partial class Test1 : Window
    {
        public Test1()
        {
            InitializeComponent();
        }

        private void HeaderGrid2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                Left = e.GetPosition(null).X - Width/2;
                Top = e.GetPosition(null).Y - 15;
            }
            DragMove();
        }  
    }
}
