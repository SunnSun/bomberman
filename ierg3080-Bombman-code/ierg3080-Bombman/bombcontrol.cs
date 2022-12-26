using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    public partial class MainWindow
    {
        private DispatcherTimer explosiontime;
        int SpawnCountdown = 30;
        int CurrentCountdown;
        int CurrentCountdown2;
        bool togglebomb = false;
        bool toggleblast = false;

        private void Bombexplode()
        {
            CurrentCountdown -= 1;
            if (CurrentCountdown < 1)
            {
                togglebomb = false;
                
                foreach (var y in GameCanvas.Children.OfType<Ellipse>())
                {
                    EllipsesToRemove.Add(y);                   
                }
            }
        }
        
        private void blastremove()
        {
            CurrentCountdown2 -= 1;
            if (CurrentCountdown2 < 15)
            {
                toggleblast = false;

                foreach (var y in GameCanvas.Children.OfType<Rectangle>())
                {
                    if((string)y.Tag == "blast")
                        ItemsToRemove.Add(y);
                }
            }
        }

        private void blasting(Ellipse bomb)
        {
            Rectangle blast = new Rectangle
            {
                Tag = "blast",
                Height = 20,
                Width = 20,
                Fill = Brushes.Red
            };
            Canvas.SetLeft(blast, Canvas.GetLeft(bomb)-5);
            Canvas.SetTop(blast, Canvas.GetTop(bomb)-7);
            
            GameCanvas.Children.Add(blast);
        }
    }
}