   M S B A M L   `   `   `  ����M  ISheetMusicEditorCS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:SheetMusicEditorCSSheetMusicEditorCS  5          SheetMusicEditorCS.App   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      2local clr-namespace:SheetMusicEditorCS     5        �� 	Resources  5       �� MergedDictionaries �� Source   StandardBorderBrush �� Color  StandardBackgroundBrush  HoverBorderBrush  SelectedBackgroundBrush  SelectedForegroundBrush  DisabledForegroundBrush  NormalBrush  NormalBorderBrush  HorizontalNormalBrush 	 HorizontalNormalBorderBrush 
 ListBoxBackgroundBrush �� 
StartPoint �� EndPoint � Color � Offset  StandardBrush  
GlyphBrush  PressedBrush  ScrollBarLineButton �� 
TargetType	 �� Property
 �� Value �� 
TargetType �� CornerRadius �� Path �� RelativeSource �� Triggers P� Property P� Value �� 
TargetName  ScrollBarPageButton  ScrollBarThumb  VerticalScrollBar � RowDefinitions ]� DecreaseRepeatButton ]� Thumb ]� IncreaseRepeatButton  HorizontalScrollBar � ColumnDefinitions �� Triggers  FavsScrollViewer �� VerticalOffset � Value �� ScrollableHeight � Maximum �� ViewportHeight �� ViewportSize* �� #ComputedVerticalScrollBarVisibility  �� Orientation! �� HorizontalOffset" �� ScrollableWidth# �� ViewportWidth,$ �� %ComputedHorizontalScrollBarVisibility�� 5   
    5	      �� 5
      $e _pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml?�6%   �� 5      $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml?�6%   �� 5      $v ppack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Purple.xaml?�6%   �� 5      $x rpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.LightBlue.xaml?�6%   %�  &	        &	      &	 <     &	 Z     &	 x     &	 �     &	 �     &	 �     &	 �     &		     &	
 -    &	     &	 �    &	 �    &	 �    7  7  7  7  7  &	 r    &	 �    &	 	    7  7  7  7  7 	 7  7  &	 �    7  7  7  7  7  7  7  '��     7  7  &	     �� 5      $
 #888��6:   �� 5      $
 #FFF��6>   �� 5      $
 #DDD��67   �� 5      $
 Gray��6>   �� 5      $ White��6>   �� 5      $
 #888��6>   �� 5      $
 #888��62   �� 5      $
 #888��68   �� 5      $
 #888��6<   �� 5      $
 #888��6B   �� 5      $	 0,0&�5      $ 1,0.001&�6   ��5      � � 5      $ White��6'   $	 0.0Q�65   � 5      $ White��6'   $	 0.6Q�65   � 5       $ #DDDDDD��6'   $	 1.2Q�67   5!      5#      �� 5$      $	 0,0&�5%      $	 0,1&�6   ��5'      � � 5(      $
 #FFF��6'   $	 0.0Q�64   � 5)      $
 #CCC��6'   $	 1.0Q�64   5*      5,      �� 5-      $
 #444��61   �� 5.      $	 0,0&�5/      $	 0,1&�6   ��51      � � 52      $
 #BBB��6'   $	 0.0Q�64   � 53      $
 #EEE��6'   $	 0.1Q�64   � 54      $
 #EEE��6'   $	 0.9Q�64   � 55      $
 #FFF��6'   $	 1.0Q�64   56      58      �� 5;      " ��60   .�5<      �� 	 �@J�SnapsToDevicePixels
 . 68   �� 5=      	 �@�OverridesDefaultStyle
 . 6:   �� 5>      	 � |�
 .  6.   �� 5?      	 � ��
 5@      �� 5A      " ��6*   .R�5B      �� $��Border��6/   $��1q�6F   $ 2��6Z   8��  8�� $
��0.25q�6�   ._�5C   "   F� $��Center=�63   $��Center=�6[   8�� ��6�   �� $ Content� �� *TemplatedParent+5D       5E      P� 5F   "    � �� . . �5G   &   �� $ Border��6-   8
  	 � ��5H   !   5I   #   P� 5J   "    � {� .  . �5K   &   �� 8
  	 � ��5L   !   5M   #   5O      5Q      5R      �� 5U      " ��60   .�5V      �� 	 �@J�SnapsToDevicePixels
 . 68   �� 5W      	 �@�OverridesDefaultStyle
 . 6:   �� 5X      	 � ��
 .  6.   �� 5Y      	 � |�
 .  6.   �� 5Z      	 � ��
 5[      �� 5\      " ��6*   .R�5]      �� 
������ 6%   5^      5`      5a      �� 5d      " n�6+   .�5e      �� 	 �@J�SnapsToDevicePixels
 . 68   �� 5f      	 �@�OverridesDefaultStyle
 . 6:   �� 5g      	 � ��
 .  6.   �� 5h      	 � |�
 .  6.   �� 5i      	 � ��
 5j      �� 5k      " n�6*   .R�5l      �� $ 2��6%   #��z��66   #��z��6`   $
��0.25q�6�   5m      5o      5p      �� 5r      " ��5s      .R�5t      � $��6��6    5u      �� 5v      $��18��6(   �� 5w      $��0.00001* �6(   �� 5x      $��18��6(   .+�5z      �� $��3��6   $ 2��6.   
������ 6?   �� 5{      $��0��6$   8��  $��18��6^   $��ScrollBar.LineUpCommand��6j   $��M 0 2 L 4 2 L 2 0 Z��6�   ]� 5|      $��
PART_Track��6   $��1��6/   ��.  5}      �� 5~      8�� $��ScrollBar.PageUpCommand��6X    5�      n� 5�      8�� $��1,0,1,0q�6L   8�� 8�� 6�    5�      �� 5�      8�� $��ScrollBar.PageDownCommand��6X   5�      �� 5�      $��3��6#   8�� $��18��6]   $��ScrollBar.LineDownCommand��6i   $��M 0 0 L 2 2 L 4 0 Z��6�   5�      5�      �� 5�      " ��5�      .R�5�      �  5�      �� 5�      $��9��6+   �� 5�      $��0.00001* �6+   �� 5�      $��9��6+   .+�5�      �� $��3��5�      $ 2��5�      ���#F0F0F05�      �� 5�      $��0��5�      8��  $��9��5�      $��ScrollBar.LineLeftCommand��5�      $��M 4 0 L 4 8 L 0 4 Z��5�      ]� 5�      $��
PART_Track��5�      $��1��5�      ��.   5�      �� 5�      8�� $��ScrollBar.PageLeftCommand��5�       5�      n� 5�      8�� $��0,1,0,1q�5�      8�� 8�� 5�       5�      �� 5�      8�� $ ��ScrollBar.PageRightCommand��5�      5�      �� 5�      $��3��5�      8�� $��9��5�      $ ��ScrollBar.LineRightCommand��5�      $��M 0 0 L 4 4 L 0 8 Z��5�      5�      5�      �� 5�      " ��6/   .�5�      �� 	 �@J�SnapsToDevicePixels
 . 68   �� 5�      	 �@�OverridesDefaultStyle
 . 6:    5�      P� 5�       �@��Orientation$ 
Horizontal=�65   . �5�      �� 	 � ��$

 Auto��62   �� 5�      	 � ��$
 18��63   �� 5�      	 � ��8
   5�      5�      P� 5�       �@��Orientation$ Vertical=�65   . �5�      �� 	 � ��$
 6��62   �� 5�      	 � ��$

 Auto��63   �� 5�      	 � ��8
  5�      5�      5�      �� 5�      " ��6-   .�5�      �� 	 �@�OverridesDefaultStyle
 . 6:   �� 5�      	 � ��
 5�      �� 5�      " ��6*   .R�5�      �  5�   "   �� 5�   &   $
��Auto �67   �� 5�   &    5�   "   �� 5�   &   �� 5�   &   $
��Auto �64   .+�5�   "   �� $��1��69   �� 5�   "   $��PART_VerticalScrollBar��6,   $��6��6J   # z 5�      # z 5�      # z 5�      #y�z 5�      �� 5�   "   $��PART_HorizontalScrollBar��6,   $  
Horizontal=�5�      $��1��5�      $��1��5�      # z! 5�      # z" 5�      # z# 5�      #y�z$ 5�      5�      5�      5�      5�      5�      5�      