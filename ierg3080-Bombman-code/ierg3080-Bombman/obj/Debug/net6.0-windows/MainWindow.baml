   M S B A M L   `   `   `  ����K  Gierg3080-Bombman, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ierg3080_Bombmanierg3080_Bombman  5   	   !    ierg3080_Bombman.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:ierg3080_Bombman     5   	     � 
ResizeMode$  NoResize=�5   	    � Title$ 
MainWindow��5	   	   $	��621��6   $	��800��6)   .��5
      � .+�5   
   �� -   5         ��Name$ 
GameCanvas��5      $��Center=�6%   $	��500��6B   $
|�True��6O   $��0,10,0,0q�6`   $	��Top=�6r   $	��680��6�   ���	#FF3C3C3C5      � 5   
   $��Center=�6   $��69��6,   $��	0,531,0,0q�68   $	��Top=�6K   $	��800��6c   
�������.+�5      ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   .+�5      �� $��60��6   $��60��6    $��Center=�6+   $��Center=�6H   $��/player.png��6c   �� 5      $��30��6   
������ 6'   �� 5      $��25��6   $��25��6    $��Center=�6+   $��Center=�6H   $��
/heart.png��6c   �� 5      -   5         ��Name$ 	Lifelabel��5      $	��100��6'   $��Center=�65   $��Center=�6R   $��Arial Black)�6m   �� 5      $��30��6   
������ 6'   �� 5      $��25��6   $��25��6    $��Center=�6+   $��Center=�6H   $��	/bomb.jpg��6c   �� 5      -   5        $ 	bomblabel��5      $��1��6'   $��Center=�63   $��Center=�6P   $��Arial Black)�6k   �� 5      $��30��6   
������ 6'   �� 5      $��25��6   $��25��6$   $��Center=�6/   $��Center=�6L   $��/blastpowerup.png��6g   �� 5      -   5        $ blastingpowerlabel��5      $��1��64   $��Center=�6@   $��Center=�6]   $��Arial Black)�6x   �� 5      $��30��6   
������ 6'   �� 5      $��25��6   $��25��6    $��Center=�6+   $��Center=�6H   $��/key.png��6c   �� 5      -   5        $ keylabel��5      $��0��6&   $��Center=�62   $��Center=�6O   $��Arial Black)�6j   �� 5      $��30��6   
������ 6'   �� 5      $��25��6   $��25��6    $��Center=�6+   $��Center=�6H   $��	/door.png��6c   �� 5      -   5        $ 	doorlabel��5      $��0��6'   $��Center=�63   $��Center=�6P   $��Arial Black)�6k   �� 5       $��30��6   
������ 6'   �� 5!      -   5        $ 
levellabel��5!      $��	level : 1��6(   $��Center=�6<   $��Center=�6Y   $��Arial Black)�6t   $��16(�6�   5"      5#      5$      5&      