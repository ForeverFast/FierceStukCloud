   M S B A M L   `   `   `  ����\  XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source   TabContolStyleMain �� 
TargetType �� Property �� Value  DimColor  GrayLVL0 �� 
TargetType-    'System.Windows.Input.KeyboardNavigation    TabNavigation � RowDefinitions'   !System.Windows.VisualStateManager   VisualStateGroups%   System.Windows.VisualStateGroup	   States    System.Windows.VisualState
   
Storyboard �� 
TargetName �� TargetProperty8  2System.Windows.Media.Animation.EasingColorKeyFrame �� KeyTime �� Value ��Name K� ZIndex K� IsItemsHost    TabIndex ��Name �� CornerRadius    DirectionalNavigation �� EndPoint �� 
StartPoint  ContentAreaColorLight � Color � Offset  ContentAreaColorDark  BorderMediumColor �� Color ��Name  Gor  
HeaderText  StandartTextColor 	 
LabelStyle �� Triggers P� Property P� Value �� 
TargetName 
 GrayLVL1  GrayLVL2�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml      5        5      �� 5   
   $G Apack://application:,,,/FierceStukCloud.Wpf;component/Brushes.xaml?�5      %  &	        7  7  7  7  7 	 7 
 7  �� 5      " ��6'   .�5   
   ��  �@��TabStripPlacement$	 Top=�6.   �� 5   
    � ��$ 0q�6$   �� 5   
    � ��$ 0q�6,   �� 5   
    � ��8   6(   �� 5   
    � ��8  6'   �� 5   
    � �� 5      �� 5      " ��6"   .R�5      � $ Local=�6    5      �� 5      $
��Auto �6,   �� 5      $��* �6,    5        5      CommonStates��.	 5    "     Disabled��.
 5!   &   �� .��5"   *   �� $ Border��6G   $ y(Border.BorderBrush).                                                                             (SolidColorBrush.Color)�5#   1   .g�5%   .     $ 0��6B   $ 	#FFAAAAAA��5&   B   5'   +   5(   '   5)   #   5*      .+�5-      �� $ HeaderPanel��6#   $��0��5.   %   $ 1��5/   %   $��0,0,0,0q�50   %   $
 True��51   %   $ 1��52   %   
������ 53   %   �� 55      $ Border��6!   $��1��56   #   $��0,0,0,0q�57   #   $ 0��59   #   $ Local=�5:   #   $ 	Contained=�5;   #   $ 2��5<   #   ��5>      �� 5?   "   $ 0.5,1&�66   $ 0.5,0&�5@   $   .��5A   &   � # �  63   $ 0Q�5B   2   � 5C   &   # �  63   $ 1Q�5D   2   5E   #   ��5H      �� 5I   "   # �  62   ._�5K      �� $ PART_SelectedContentHost��6/   $��0q�5L   /   $��SelectedContent��5M   /   5N      5O      5Q      5S      �� 5U   
    � �� 5V      �� 5W      " ��6   .�5Y      ��  � ��$ 0,0,0,0q�6/   �� 5\       � ��$	 150��6.   �� 5]       � ��$ 45��6/   �� 5`       � �� 5a      �� 5b      " ��6.   .R�5c   "   �� $ Border��6)   8�� ._�5f   &   �� #���  6,   $��15(�5g   ,   8�� $��5,0,0,0q�5j   ,   8�� 5k   ,   5l   #    5n   "   P� 5o   &    �@��
IsSelected$
 True��6D   . �5p   *   �� $ Border��61   8   � ��6[   5q   '   P� 5r   &    �@��
IsSelected$ False��6D   . �5s   *   �� $ Border��61   8   � ��6[   5t   '   5v      5x      5y      5{      5}      5~      