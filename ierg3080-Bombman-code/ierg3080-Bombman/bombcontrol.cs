using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
        int togglebomb = 0;
        int toggleblast = 0;
        int blastingpower = 1;
        public List<Rectangle> BlastGrids = new List<Rectangle>();

        private void Bombexplode(double bombx, double bomby)
        {
            togglebomb--;
            toggleblast++;
            foreach (var y in GameCanvas.Children.OfType<Rectangle>())
            {
                if (Canvas.GetLeft(y) == bombx && Canvas.GetTop(y) == bomby)
                    ItemsToRemove.Add(y);
            }
        }

        public void blastremove(double bombx, double bomby)
        {
            foreach (var y in GameCanvas.Children.OfType<Rectangle>())
            {
                if ((string)y.Tag == "blast")
                    ItemsToRemove.Add(y);
            }
        }
        private void blasting(double bombx, double bomby)
        {
            List<Rectangle> blast = new List<Rectangle>();
            for (int i = 0; i < blastingpower * 4 + 1; i++)
            {
                Rectangle rec = new Rectangle
                {
                    Tag = "blast",
                    Height = 20,
                    Width = 20,
                    Fill = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/ierg3080-Bombman;component/blast.png", UriKind.Absolute))

                    }
                    //Fill = Brushes.Red
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
                        //left -5 top - 6
                        Canvas.SetLeft(rec, bombx);
                        Canvas.SetTop(rec, bomby - count * 20); break;
                    case 1:
                        Canvas.SetLeft(rec, bombx);
                        Canvas.SetTop(rec, bomby + count * 20); break;
                    case 2:
                        Canvas.SetLeft(rec, bombx - count * 20);
                        Canvas.SetTop(rec, bomby); break;
                    case 3:
                        Canvas.SetLeft(rec, bombx + count * 20);
                        Canvas.SetTop(rec, bomby); break;
                    default:
                        Canvas.SetLeft(rec, bombx);
                        Canvas.SetTop(rec, bomby); break;
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


                bombdestroy(Canvas.GetLeft(rec), Canvas.GetTop(rec), 20, 20);
            }
            toggleblast--;
        }


        /*private void passbomb(Ellipse bomb)
        //please change the Ellipse to rectangle
        {
            if (CurrentCountdown < 15)
            {
                blasting();
            }
        }*/

        private void bombdestroy(double wallx, double wally, int width, int height)
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
                        thingsBehindBlocks(wallx, wally, width, height);
                    }
                }
                if ((string)y.Tag == "enemy")
                {
                    Rect hitenemy = new Rect(Canvas.GetLeft(y) + 5, Canvas.GetTop(y) + 5, 10, 10);
                    if (hitenemy.IntersectsWith(hitblast))
                    {
                        ItemsToRemove.Add(y);
                    }
                }
                if ((string)y.Tag == "bombpowerup")
                {
                    Rect hitbombpowerup = new Rect(Canvas.GetLeft(y) + 5, Canvas.GetTop(y) + 5, 10, 10);
                    if (hitbombpowerup.IntersectsWith(hitblast))
                    {
                        ItemsToRemove.Add(y);
                    }
                }
                if ((string)y.Tag == "blastpowerup")
                {
                    Rect hitblastpowerup = new Rect(Canvas.GetLeft(y) + 5, Canvas.GetTop(y) + 5, 10, 10);
                    if (hitblastpowerup.IntersectsWith(hitblast))
                    {
                        ItemsToRemove.Add(y);
                    }
                }
                if ((string)y.Tag == "breakablewall" && map[(int)Canvas.GetTop(y) / 20, (int)Canvas.GetLeft(y) / 20] == 'D')
                {
                    Rect hitblastpowerup = new Rect(Canvas.GetLeft(y) + 5, Canvas.GetTop(y) + 5, 10, 10);
                    if (hitblastpowerup.IntersectsWith(hitblast))
                    {
                        ItemsToRemove.Add(y);
                        generateDoor(Canvas.GetLeft(y), Canvas.GetTop(y), width, height);
                    }
                }
                if ((string)y.Tag == "breakablewall" && map[(int)Canvas.GetTop(y) / 20, (int)Canvas.GetLeft(y) / 20] == 'K')
                {
                    Rect hitblastpowerup = new Rect(Canvas.GetLeft(y) + 5, Canvas.GetTop(y) + 5, 10, 10);
                    if (hitblastpowerup.IntersectsWith(hitblast))
                    {
                        ItemsToRemove.Add(y);
                        generateKey(Canvas.GetLeft(y), Canvas.GetTop(y), width, height);
                    }

                }
            }
        }

        private async void generateKey(double wallx, double wally, int width, int height)
        {
            Rectangle Key = new Rectangle
            {
                Tag = "key",
                Height = height,
                Width = width,
                Fill = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/ierg3080-Bombman;component/key.png", UriKind.Absolute))

                }
                //Fill = Brushes.Blue
            };
            Canvas.SetLeft(Key, wallx);
            Canvas.SetTop(Key, wally);
            await Task.Delay(1000);
            GameCanvas.Children.Add(Key);
        }

        private async void generateDoor(double wallx, double wally, int width, int height)
        {
            Rectangle Door = new Rectangle
            {
                Tag = "door",
                Height = height,
                Width = width,
                Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/ierg3080-Bombman;component/door.png", UriKind.Absolute)) }
                //Fill = Brushes.Brown
            };
            Canvas.SetLeft(Door, wallx);
            Canvas.SetTop(Door, wally);
            await Task.Delay(1000);
            GameCanvas.Children.Add(Door);
            doorlabel.Content = 1;
        }
        private async void generateBombPowerUp(double wallx, double wally, int width, int height)
        {
            Rectangle BombPowerUp = new Rectangle
            {
                Tag = "bombpowerup",
                Height = height,
                Width = width,
                Fill = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/ierg3080-Bombman;component/bombpowerup.png", UriKind.Absolute))

                }
                //Fill = Brushes.Pink
            };
            Canvas.SetLeft(BombPowerUp, wallx);
            Canvas.SetTop(BombPowerUp, wally);
            await Task.Delay(1000);
            GameCanvas.Children.Add(BombPowerUp);
        }
        private async void generateBlastPowerUp(double wallx, double wally, int width, int height)
        {
            Rectangle BlastPowerUp = new Rectangle
            {
                Tag = "blastpowerup",
                Height = height,
                Width = width,
                Fill = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/ierg3080-Bombman;component/blastpowerup.png", UriKind.Absolute))

                }
                //Fill = Brushes.Green
            };
            Canvas.SetLeft(BlastPowerUp, wallx);
            Canvas.SetTop(BlastPowerUp, wally);
            await Task.Delay(1000);
            GameCanvas.Children.Add(BlastPowerUp);
        }
        private void thingsBehindBlocks(double wallx, double wally, int width, int height)
        {
            /*foreach (var y in GameCanvas.Children.OfType<Rectangle>())
            {
                if ((string)y.Tag == "breakablewall")
                {
                    countbreakablewall++;
                }
            }
            if (countbreakablewall == 2 && isGeneratedDoor == false && isGeneratedKey == false)
            {
                generateKey(wallx, wally, width, height);
            }
            if (countbreakablewall == 1 && isGeneratedDoor == true && isGeneratedKey == false)
            {
                generateKey(wallx, wally, width, height);
            }
            if (countbreakablewall == 1 && isGeneratedDoor == false && isGeneratedKey == true)
            {
                generateDoor(wallx, wally, width, height);
            }
            */
            int itemSpawnProbiblity = rand.Next(1, 100);
            /*if (itemSpawnProbiblity > 80 && itemSpawnProbiblity <= 85 && isGeneratedKey == false)
            {
                generateKey(wallx, wally, width, height);
                isGeneratedKey = true;
            }
            else if (itemSpawnProbiblity > 86 && itemSpawnProbiblity <= 90 && isGeneratedDoor == false)
            {
                generateDoor(wallx, wally, width, height);
                isGeneratedDoor = true;
            }
            else */
            if (itemSpawnProbiblity > 90 && itemSpawnProbiblity <= 95 && map[(int)wally / 20, (int)wallx / 20] == 'B')
            {
                generateBombPowerUp(wallx, wally, width, height);
            }
            else if (itemSpawnProbiblity > 95 && itemSpawnProbiblity <= 100 && map[(int)wally / 20, (int)wallx / 20] == 'B')
            {
                generateBlastPowerUp(wallx, wally, width, height);
            }
        }
    }
}