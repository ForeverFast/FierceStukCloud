   M S B A M L   `   `   `  ����N  JFierceStukCloud.Wpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:FierceStukCloud.WpfFierceStukCloud.Wpf  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089V RMaterialDesignThemes.Wpf, Version=3.1.3.1513, Culture=neutral, PublicKeyToken=null  �� MergedDictionaries �� Source   ScrollBarLineButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType'   !MaterialDesignThemes.Wpf.PackIcon    Kind  StandartLightColor " ScrollBarLineButtonBottomStyle  ScrollBarPageButtonStyle  ScrollBarThumbStyle  GrayLVL0  VerticalScrollBar � RowDefinitions ��Name	 ]� DecreaseRepeatButton
 ]� Thumb ]� IncreaseRepeatButton �� Triggers P� 
SourceName P� Property P� Value �� 
TargetName  HorizontalScrollBar � ColumnDefinitions  DefaultScrollBar �� Triggers�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      +local!clr-namespace:FierceStukCloud.Wpf  5      :md1http://materialdesigninxaml.net/winfx/xaml/themes  5        5      �� 5   
   $G Apack://application:,,,/FierceStukCloud.Wpf;component/Brushes.xaml?�5      %   &	        7  &	 �     7  &	 �    &	 �    7  &	 �    7   7  7  7  7  &	 �    7   7  7  7  7  &	 �    7  7  �� 5      " ��6-   .�5   
   ��  � |�$ False��6&   �� 5   
    � �� 5      �� 5      " ��6"   .R�5      �� .+�5         $ KeyboardArrowUp=�6&   8��  6=   5      5      5      5      �� 5      " ��63   .�5   
   ��  � |�$ False��6&   �� 5   
    � �� 5      �� 5       " ��6"   .R�5!      �� .+�5"         $ KeyboardArrowDown=�6&   8��  6?   5#      5$      5&      5'      �� 5)      " ��6-   .�5*   
   ��  � ��$ False��6&   �� 5+   
    � |�$ False��6&   �� 5,   
    � �� 5-      �� 5.      " ��6"   .R�5/      �� 
������ 69   50      52      53      �� 55      " n�6(   .�56   
   ��  � ��$ False��6&   �� 57   
    � |�$ False��6&   �� 58   
    � ��$ 1,0,1,0q�6#   �� 59   
    � �� 5:      �� 5;      " n�6"   .R�5<      �� 8��  $��2q�6A   6N   5=      5?      5@      �� 5B      " ��60   .R�5C   
   �  5D      �� 5E      $��18��6    �� 5F      $��* �6    �� 5G      $��16��6    .+�5K      �� $
 RBup��64   $��0��6   $��18��6(   8��  $��ScrollBar.LineUpCommand��5M      5N      ]� 5O      $��
PART_Track��6   $��1��6&   $
��True��5P      	 5Q      �� 5R      $��ScrollBar.PageUpCommand��6#   8�� 5S      
 5U      n� 5V      8�� 5W       5Y      �� 5Z      $��ScrollBar.PageDownCommand��6#   8�� 5[      5]      �� 5^      $ RBdown��64   $��3��6   $��16��6(   8�� $��ScrollBar.LineDownCommand��5`      5a      5b       5c   
   P� 5d      $ 
PART_Track��6,    � y�$ Hidden=�6D   . �5e      �� $
 RBup��6/    � y�$ Hidden=�6A   �� 5f      $ RBdown��6/    � y�$ Hidden=�6C   5g      P� 5h      $ 
PART_Track��6,    � y�$ Visible=�6D   . �5i      �� $
 RBup��6/    � y�$ Visible=�6A   �� 5j      $ RBdown��6/    � y�$ Visible=�6C   5k      5m      �� 5o      " ��62   .R�5p   
   �  5q      �� 5r      $��18��6#   �� 5s      $��* �6#   �� 5t      $��16��6#   .+�5x      �� $
 RBup��66   $��0��6   $��18��6+   8��  $��ScrollBar.LineLeftCommand��5z      5{      ]� 5|      $��
PART_Track��6   $��1��6&   $
��True��5}      	 5~      �� 5      $��ScrollBar.PageLeftCommand��6#   8�� 5�      
 5�      n� 5�      8�� 5�       5�      �� 5�      $ ��ScrollBar.PageRightCommand��6#   8�� 5�      5�      �� 5�      $ RBdown��66   $��3��6   $��16��6+   8�� $ ��ScrollBar.LineRightCommand��5�      5�      5�       5�   
   P� 5�      $ 
PART_Track��6,    � y�$ Hidden=�6D   . �5�      �� $
 RBup��6/    � y�$ Hidden=�6A   �� 5�      $ RBdown��6/    � y�$ Hidden=�6C   5�      P� 5�      $ 
PART_Track��6,    � y�$ Visible=�6D   . �5�      �� $
 RBup��6/    � y�$ Visible=�6A   �� 5�      $ RBdown��6/    � y�$ Visible=�6C   5�      5�      �� 5�      " ��6%    5�   
   P� 5�       �@��Orientation$ 
Horizontal=�6-   . �5�      ��  � ��$
 Auto��6*   �� 5�       � ��$ 18��6+   �� 5�       � ��8   6-   5�      P� 5�       �@��Orientation$ Vertical=�6-   . �5�      ��  � ��$ 18��6*   �� 5�       � ��$
 Auto��6+   �� 5�       � ��8  6-   5�      5�      5�      