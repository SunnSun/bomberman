using System;
using System.Collections;
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

                foreach (var y in GameCanvas.Children.OfType<Rectangle>())
                {
                    if((string)y.Tag == "bomb")
                    ItemsToRemove.Add(y);                   
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
                    if((string)y.Tag == "blast")
                        ItemsToRemove.Add(y);
                }
            }
        }

        private void blasting()
        {
            List<Rectangle> blast = new List<Rectangle>();
            for(int i = 0; i < blastingpower * 4 + 1; i++)
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
            foreach(Rectangle rec in blast)
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
                if(count > blastingpower)
                {
                    count = 1;
                    dir++;
                }
                GameCanvas.Children.Add(rec);
                bombdestroywall(Canvas.GetLeft(rec), Canvas.GetTop(rec), 20, 20);
            }
            //0 = up, 1 = down, 2 = left, 3 = right
           /* Rectangle blast = new Rectangle
            {
                Tag = "blast",
                Height = 20,
                Width = 20,
                Fill = Brushes.Red
            };
            Canvas.SetLeft(blast, bombx - 5);
            Canvas.SetTop(blast, bomby - 6);
            GameCanvas.Children.Add(blast);
            bombdestroywall(Canvas.GetLeft(blast), Canvas.GetTop(blast), 20, 20);
            for (int dir = 0; dir < 4; dir++)
            {
                for (int i = 1; i <= blastingpower; i++)
                {
                    Rectangle blast1 = new Rectangle
                    {
                        Tag = "blast",
                        Height = 20,
                        Width = 20,
                        Fill = Brushes.Red
                    };
                    switch (dir)
                    {
                        case 0: Canvas.SetLeft(blast, bombx - 5);
                                Canvas.SetTop(blast, bomby - i*20 - 6); break;
                        case 1: Canvas.SetLeft(blast, bombx - 5);
                                Canvas.SetTop(blast, bomby + i * 20 - 6); break;
                        case 2: Canvas.SetLeft(blast, bombx - i * 20 - 5);
                                Canvas.SetTop(blast, bomby - 6); break;
                        case 3: Canvas.SetLeft(blast, bombx + i * 20 - 5);
                                Canvas.SetTop(blast, bomby - 6); break;

                    }
                    GameCanvas.Children.Add(blast1);
                    bombdestroywall(Canvas.GetLeft(blast1), Canvas.GetTop(blast1), 20, 20);
                    Canvas.SetLeft(blast, bombx - 5);
                    Canvas.SetTop(blast, bomby - 20 * blastingpower - 6);
                }
            }*/
            /*Rectangle blast1 = new Rectangle
            {
                Tag = "blast",
                Height = 20 + blastingpower * 40,
                Width = 20,
                Fill = Brushes.Red
            };
            //Canvas.GetLeft(Player) Canvas.GetTop(Player)
            Canvas.SetLeft(blast1, bombx-5);
                            Canvas.SetTop(blast1, bomby - 20 * blastingpower -6);
                            Rectangle blast = new Rectangle
                            {
                                Tag = "blast",
                                Height = 20,
                                Width = 20 + blastingpower * 40,
                                Fill = Brushes.Red
                            };
            Canvas.SetLeft(blast, bombx - 20 * blastingpower - 5);
                            Canvas.SetTop(blast, bomby-6);
                            GameCanvas.Children.Add(blast);
                            bombdestroywall(Canvas.GetLeft(blast), Canvas.GetTop(blast), 40*blastingpower+20, 20);
            GameCanvas.Children.Add(blast1);
                            bombdestroywall(Canvas.GetLeft(blast1), Canvas.GetTop(blast1), 20, 40 * blastingpower + 20);*/
            toggleblast = false;
        }

        private void passbomb(Rectangle bomb)
        {
            if(CurrentCountdown < 15)
            {
                blasting();
            }
        }

        private void bombdestroywall(double wallx, double wally, int width, int height)
        {
            Rect hitblast = new Rect(wallx, wally, width, height);
            foreach(var y in GameCanvas.Children.OfType<Rectangle>())
            {
                if ((string)y.Tag == "breakablewall")
                {
                    Rect hitbreakablewall = new Rect(Canvas.GetLeft(y) + 5, Canvas.GetTop(y) +5, 10, 10);
                    if(hitbreakablewall.IntersectsWith(hitblast))
                    {
                        ItemsToRemove.Add(y);
                        thingsBehindBlocks(wallx, wally, width, height);
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
                    Height = 20,
                    Width = 20,
                    Fill = Brushes.Yellow
                };
        }

        private void generateDoor(double wallx, double wally, int width, int height)
        {

        }
        private void generateBombPowerUp(double wallx, double wally, int width, int height)
        {

        }
        private void generateBlastPowerUp(double wallx, double wally, int width, int height)
        {

        }
        private void thingsBehindBlocks(double wallx, double wally, int width, int height)
        {
            
            int itemSpawnProbiblity = rand.Next(1, 100);
            for (int i = 0; i < 100; i++)
            {
                if (itemSpawnProbiblity > 80 && itemSpawnProbiblity <= 85 && isGeneratedKey)
                {
                    generateKey(wallx, wally, width, height);
                }
                else if (itemSpawnProbiblity > 86 && itemSpawnProbiblity <= 90 && isGeneratedDoor)
                {
                    generateDoor(wallx, wally, width, height);
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
}