   M S B A M L   `   `   `  ����  AKRemindClient    AKRemindClient.ShowWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  
ShowWindow��5   	    � WindowStyle$
 None=�6   $	��200��6/   $��55��6;    � Topmost .  � AllowsTransparency .  � Icon$ AKRemind.ico��6p   
���   � � 	Resources 5         OuterGlowStyle �� 
TargetType �� Triggers :� RoutedEvent	 �� TargetProperty
 g� 	BeginTime W� From	 W� To g� Duration g� AutoReverse g� RepeatBehavior��%  &	        �� 5   
   " �6'    5      :� 5      $ Loaded��6   .[�5	      �� .��5
      �� .��5      W� $	 Opacity�6.   $
 00:00:00X�6R   $	 0.5X�6g   $ 1X�6r   $ 00:00:01D�6y    . $ Forever��5      5      5      5      5      5      .��5      � #��[  6   ��5   
   �� 5       �� ImageSource$ fenx.png��6    j� Stretch$
 Fill=�60   6A   5)      5*      