   M S B A M L   `   `   `  ����I  EAKRemindReport, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:AKRemindReportAKRemindReport  5   	       AKRemindReport.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   7akRemindReportclr-namespace:AKRemindReport     5   	     � Title$  分线报表��5   	   $	��350��6   $	��525��6#    � Icon$ AKRemind.ico��6/   $y�Visible=�6D    � WindowStartupLocation$ CenterScreen=�6Y   .��5      � .+�5   
   �� $��15q�6   .+�5      ��  �� Orientation$ 
Horizontal=�6   $��0,0,0,15q�63   .+�5	      �� $��开始日期:��6   $��Center=�6)   6F   (  "System.Windows.Controls.DatePicker  5
      -   5        $��dtStart��5
   1   $��
3,10,10,10q�6   $	��100��6@   �� 5      $��Center=�6   结束日期:6>     5      -   5        $��dtEnd��5   1   $��
3,10,10,10q�6   $	��100��6>   5      �� 5      $ 
Horizontal=�6   .+�5      �� $��选择导出条件：��6   6-   �� 5      -   5        $��	checkBox1��5      $��早餐��6,    d� 	IsChecked$
 TrueY�69   6L   �� 5      -   5        $��	checkBox2��5      $��午餐��6,   $
 TrueY�69   6L   �� 5      -   5        $��	checkBox3��5      $��	下午茶��6,   $
 TrueY�6:   6M   �� 5      -   5        $��	checkBox4��5      $��晚餐��6,   $
 TrueY�69   6L   5      �� 5      $ 
Horizontal=�6   $��0,20,0,0q�63   .+�5      �� -   5        $��导出Excel��5      
��� ��$��50��6C   $	��100��6O   6r   5      5          AKRemindReport.LoadingWait  5   
   -   5          Name$ _loading��5   %   $y�	Collapsed=�68   5      5      