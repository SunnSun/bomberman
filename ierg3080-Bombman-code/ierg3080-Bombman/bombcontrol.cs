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
        int blastingpower = 2;
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

                foreach (var y in GameCanvas.Children.OfType<Rectangle>())
                {
                    if ((string)y.Tag == "blast")
                        ItemsToRemove.Add(y);
                }
            }
        }
        private void blasting()
        {
            List<Rectangle> blast = new List<Rectangle>();
            for (int i = 0; i < blastingpower * 4 + 1; i++)
            {
                Rectangle rec = new Rectangle
                {
                    Tag = "blast",
                    Height = 20,
                    Width = 20,
                    Fill = Brushes.Red
                };
                blast.Add(rec);
            }
            int dir = 0;
            int count = 1;
            foreach (Rectangle rec in blast)
            {
                switch (dir)
                {
                    case 0:
                        Canvas.SetLeft(rec, bombx - 5);
                        Canvas.SetTop(rec, bomby - count * 20 - 6); break;
                    case 1:
                        Canvas.SetLeft(rec, bombx - 5);
                        Canvas.SetTop(rec, bomby + count * 20 - 6); break;
                    case 2:
                        Canvas.SetLeft(rec, bombx - count * 20 - 5);
                        Canvas.SetTop(rec, bomby - 6); break;
                    case 3:
                        Canvas.SetLeft(rec, bombx + count * 20 - 5);
                        Canvas.SetTop(rec, bomby - 6); break;
                    default:
                        Canvas.SetLeft(rec, bombx - 5);
                        Canvas.SetTop(rec, bomby - 6); break;
                }
                count++;
                if (count > blastingpower)
                {
                    count = 1;
                    dir++;
                }
                else
                {
                    foreach (var y in GameCanvas.Children.OfType<Rectangle>())
                    {
                        if ((string)y.Tag == "wall" || (string)y.Tag == "breakablewall")
                        {
                            Rect hitblast = new Rect(Canvas.GetLeft(rec), Canvas.GetTop(rec), 20, 20);
                            Rect hitbreakablewall = new Rect(Canvas.GetLeft(y) + 5, Canvas.GetTop(y) + 5, 10, 10);
                            if (hitbreakablewall.IntersectsWith(hitblast))
                            {
                                dir++;
                                count = 1;
                            }
                        }
                    }
                }
                GameCanvas.Children.Add(rec);


                bombdestroywall(Canvas.GetLeft(rec), Canvas.GetTop(rec), 20, 20);
            }
            toggleblast = false;
        }
        //test


        private void passbomb(Ellipse bomb)
        //please change the Ellipse to rectangle
        {
            if (CurrentCountdown < 15)
            {
                blasting();
            }
        }

        private void bombdestroywall(double wallx, double wally, int width, int height)
        {
            Rect hitblast = new Rect(wallx, wally, width, height);
            foreach (var y in GameCanvas.Children.OfType<Rectangle>())
            {
                if ((string)y.Tag == "breakablewall")
                {
                    Rect hitbreakablewall = new Rect(Canvas.GetLeft(y) + 5, Canvas.GetTop(y) + 5, 10, 10);
                    if (hitbreakablewall.IntersectsWith(hitblast))
                    {
                        ItemsToRemove.Add(y);
                    }
                }
                if ((string)y.Tag == "enemy")
                {
                    Rect hitbreakablewall = new Rect(Canvas.GetLeft(y) + 5, Canvas.GetTop(y) + 5, 10, 10);
                    if (hitbreakablewall.IntersectsWith(hitblast))
                    {
                        ItemsToRemove.Add(y);
                    }
                }
            }
        }

        private void generateKey(double wallx, double wally, int width, int height)
        {
            Rectangle Key = new Rectangle
            {
                Tag = "key",
                Height = height,
                Width = width,
                Fill = Brushes.Blue
            };
            Canvas.SetLeft(Key, wallx);
            Canvas.SetTop(Key, wally);
        }

        private void generateDoor(double wallx, double wally, int width, int height)
        {
            Rectangle Door = new Rectangle
            {
                Tag = "door",
                Height = height,
                Width = width,
                Fill = Brushes.Brown
            };
            Canvas.SetLeft(Door, wallx);
            Canvas.SetTop(Door, wally);
        }
        private void generateBombPowerUp(double wallx, double wally, int width, int height)
        {
            Rectangle BombPowerUp = new Rectangle
            {
                Tag = "bombpowerup",
                Height = height,
                Width = width,
                Fill = Brushes.Pink
            };
            Canvas.SetLeft(BombPowerUp, wallx);
            Canvas.SetTop(BombPowerUp, wally);
        }
        private void generateBlastPowerUp(double wallx, double wally, int width, int height)
        {
            Rectangle BlastPowerUp = new Rectangle
            {
                Tag = "blastpowerup",
                Height = height,
                Width = width,
                Fill = Brushes.Green
            };
            Canvas.SetLeft(BlastPowerUp, wallx);
            Canvas.SetTop(BlastPowerUp, wally);
        }
        bool isGeneratedKey, isGeneratedDoor;
        private void thingsBehindBlocks(double wallx, double wally, int width, int height)
        {
            int itemSpawnProbiblity = rand.Next(1, 100);
            if (itemSpawnProbiblity > 80 && itemSpawnProbiblity <= 85 && isGeneratedKey == false)
            {
                generateKey(wallx, wally, width, height);
                isGeneratedKey = true;
            }
            else if (itemSpawnProbiblity > 86 && itemSpawnProbiblity <= 90 && isGeneratedDoor == false)
            {
                generateDoor(wallx, wally, width, height);
                isGeneratedDoor = true;
            }
            else if (itemSpawnProbiblity > 90 && itemSpawnProbiblity <= 95)
            {
                generateBombPowerUp(wallx, wally, width, height);
            }
            else if (itemSpawnProbiblity > 95 && itemSpawnProbiblity <= 100)
            {
                generateBlastPowerUp(wallx, wally, width, height);
            }
        }
    }
}