using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ierg3080_Bombman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        bool MoveUp, MoveDown, MoveLeft, MoveRight;

        

        DispatcherTimer GameTimer = new DispatcherTimer();



        public MainWindow()
        {
            InitializeComponent();
            MapSetup();
            randomwallgenerate();
            GameCanvas.Focus();
            GameTimer.Tick += GameLoop;
            GameTimer.Interval = TimeSpan.FromMilliseconds(100);
            
            GameTimer.Start();

        }



        private void Restart_buttonclick(object sender, RoutedEventArgs e)
        {
            GameTimer.Stop();
            GameTimer.Tick -= GameLoop;
            MapSetup();
            GameCanvas.Focus();
            GameTimer.Tick += GameLoop;
            GameTimer.Interval = TimeSpan.FromMilliseconds(100);

            GameTimer.Start();
            randomwallgenerate();
        }

        
        

        //testing code change
        //testing code change #2
    }
}
