﻿#pragma checksum "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A3BB0E7E6169FE9789E228455AD86ECB0B10D913"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using FierceStukCloud.Wpf.Converters;
using FierceStukCloud_PC.MVVM.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace FierceStukCloud_PC.MVVM.Views {
    
    
    /// <summary>
    /// AuthorizationV
    /// </summary>
    public partial class AuthorizationV : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 47 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid HeaderGrid;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label FSC;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MinimizeButton;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CloseWindowButton;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid AuthenticationGrid;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock loginPlace;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox loginTextBox;
        
        #line default
        #line hidden
        
        
        #line 131 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock passwordPlace;
        
        #line default
        #line hidden
        
        
        #line 138 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox passwordBox;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        
        #line 110 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"

                            private void loginBox_TextChanged(object sender, RoutedEventArgs e)
                            {
                                loginPlace.Visibility = Visibility.Visible;
                                loginPlace.Visibility = string.IsNullOrEmpty(loginTextBox.Text)
                                ? Visibility.Visible
                                : Visibility.Hidden;
                            }
                            
                            private void loginTextBox_GotFocus(object sender, RoutedEventArgs e)
                            {
                                loginPlace.Visibility = Visibility.Hidden;
                            }
                        
        #line default
        #line hidden
        
        
        #line 145 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"

                            private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
                            { 
                                passwordPlace.Visibility = Visibility.Visible;
                                passwordPlace.Visibility = string.IsNullOrEmpty(passwordBox.Password)
                                ? Visibility.Visible
                                : Visibility.Hidden;
                            }
                        
                            private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
                            {
                                passwordPlace.Visibility = Visibility.Hidden;
                            }
                        
        #line default
        #line hidden
        
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/FierceStukCloud_Pс;component/mvvm/views/authenticationv.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.HeaderGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.FSC = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.MinimizeButton = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.CloseWindowButton = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.AuthenticationGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.loginPlace = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.loginTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 103 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"
            this.loginTextBox.GotFocus += new System.Windows.RoutedEventHandler(this.loginTextBox_GotFocus);
            
            #line default
            #line hidden
            
            #line 104 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"
            this.loginTextBox.LostFocus += new System.Windows.RoutedEventHandler(this.loginBox_TextChanged);
            
            #line default
            #line hidden
            
            #line 105 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"
            this.loginTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.loginBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.passwordPlace = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.passwordBox = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 139 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"
            this.passwordBox.GotFocus += new System.Windows.RoutedEventHandler(this.passwordBox_GotFocus);
            
            #line default
            #line hidden
            
            #line 140 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"
            this.passwordBox.LostFocus += new System.Windows.RoutedEventHandler(this.passwordBox_PasswordChanged);
            
            #line default
            #line hidden
            
            #line 141 "..\..\..\..\..\MVVM\Views\AuthenticationV.xaml"
            this.passwordBox.PasswordChanged += new System.Windows.RoutedEventHandler(this.passwordBox_PasswordChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

