   M S B A M L   `   `   `  ����N  JFierceStukCloud.Wpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:FierceStukCloud.Wpf.Styles.Button!FierceStukCloud.Wpf.Styles.Button  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   LVI_PlayButtonTemplate  �� 
TargetType ��Name  GorWithLowOpacity �� CornerRadius ��Name
 � Tag  StandartLightColor2 �� Padding �� Triggers P� Property P� Value	 �� 
TargetName
 �� Property �� Value �� Value �� Binding  LVI_PlayButton �� 
TargetType �� Triggers ^� 
Conditions �� Binding �� Value�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      9local/clr-namespace:FierceStukCloud.Wpf.Styles.Button  5      %E  &	        7  7  &	 i    7   �� 5      "  ��65   .R�5   
   �� $ Border��6   8��  $ 25��5
      $��34��5      $	��130��5      ._�5      �� $ 
ButtonText��65   #��z 6   $
��14.5(�5      8�� $ 0q�5      $��Center=�5      $��Center=�5      5       5   
   P� 5       �@J�IsMouseOver$
 True��6-   . �5      �� $	 Border��66   
 � ��$	 136��6*   66   �� 5      $	 Border��66   
 � ��$ 36��6+   66   �� 5      $	 
ButtonText��68   
 � ��$ 16(�6-   68   �� 5      $	 
ButtonText��6=   
 � ��
 �����5      �� 5      $
 True��68    6   �� *	IsPlaying+.a�5      �� $	 
ButtonText��67   
 � ��$ 
Пауза��6)   67   5       �� 5!      $ False��68    6   �� *	IsPlaying+.a�5"      �� $	 
ButtonText��69   
 � ��$ Слушать��6)   69   5#      5&      �� 5(      " ��6#   .�5)   
   �� 
 � ��8   6%    5*   
   ^� 5,       5-      �� 5.       6    �� *	IsPlaying+	 False6>   �� 5/       6    �� *IsCurrentMusicContainer+ True6L   0�51      �� 52      
 � �� 60   �� *PlayStateSongCommand+54      ^� 56       57      �� 58       6    �� *	IsPlaying+ True6>   �� 59       6    �� *IsCurrentMusicContainer+ True6L   0�5;      �� 5<      
 � �� 60   �� *PlayStateSongCommand+5>      �� 5@       6   �� *IsCurrentMusicContainer+	 False6F   .a�5A      �� 
 � �� 6,   �� *ListenPlaylistsCommand+5B      5D      5F      