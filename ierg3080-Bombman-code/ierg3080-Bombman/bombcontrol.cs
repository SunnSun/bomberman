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
        
        int SpawnCountdown = 30;
        int CurrentCountdown;
        int CurrentCountdown2;
        bool togglebomb = false;
        bool toggleblast = false;
        int blastingpower = 1;
        public List<Rectangle> BlastGrids = new List<Rectangle>();

        private void Bombexplode()
        {
            CurrentCountdown -= 1;
            if (CurrentCountdown < 15)
            {
                togglebomb = false;
                toggleblast = true;
                CurrentCountdown2 = SpawnCountdown;

                foreach (var y in GameCanvas.Children.OfType<Ellipse>())
                {
                    EllipsesToRemove.Add(y);                   
                }
            }
        }
        
        public void blastremove()
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
                    Canvas.SetLeft(blast, Canvas.GetLeft(bomb) - 5);
                    Canvas.SetTop(blast, Canvas.GetTop(bomb) - 7);

                    GameCanvas.Children.Add(blast);
              
        }
    }
}