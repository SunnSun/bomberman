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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ierg3080_Bombman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// test
    public partial class MainWindow
    {
        Rect Playerhitboxleft, Playerhitboxright, Playerhitboxup, Playerhitboxdown;
        int enemeymovement = 1;
        int playerLife = 100;
        bool playerWithKey = false;
        int bombmaximum = 2;
        bool plantedbomb = false;
        private async void keyreleased(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                MoveUp = false;
                NoDown = false;
                NoLeft = false;
                NoRight = false;
            }
            if (e.Key == Key.S)
            {
                MoveDown = false;
                NoUp = false;
                NoLeft = false;
                NoRight = false;
            }
            if (e.Key == Key.A)
            {
                MoveLeft = false;
                NoDown = false;
                NoUp = false;
                NoRight = false;
            }
            if (e.Key == Key.D)
            {
                MoveRight = false;
                NoDown = false;
                NoLeft = false;
                NoUp = false;
            }
            if (togglebomb < bombmaximum)
            {
                if (e.Key == Key.Space)
                {
                    Rectangle Bomb = new Rectangle
                    {
                        Tag = "Bomb",
                        Height = 20,
                        Width = 20,
                        //Stroke = Brushes.Black,
                        //StrokeThickness = 1,
                        Fill = Brushes.Blue
                    };
                    Canvas.SetTop(Bomb, Canvas.GetTop(Player)-5);
                    Canvas.SetLeft(Bomb, Canvas.GetLeft(Player)-5);
                    GameCanvas.Children.Add(Bomb);
                    togglebomb++;
                    await Task.Delay(2000);
                    Bombexplode(Canvas.GetLeft(Bomb), Canvas.GetTop(Bomb));
                    blasting(Canvas.GetLeft(Bomb), Canvas.GetTop(Bomb));
                    await Task.Delay(1000);
                    blastremove(Canvas.GetLeft(Bomb), Canvas.GetTop(Bomb));
                }
            }

        }

        private void keypressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W && NoUp == false)
            {
                MoveUp = true;
                NoDown = true;
                NoLeft = true;
                NoRight = true;
            }
            if (e.Key == Key.S && NoDown == false)
            {
                MoveDown = true;
                NoUp = true;
                NoLeft = true;
                NoRight = true;
            }
            if (e.Key == Key.A && NoLeft == false)
            {
                MoveLeft = true;
                NoDown = true;
                NoUp = true;
                NoRight = true;
            }
            if (e.Key == Key.D && NoRight == false)
            {
                MoveRight = true;
                NoDown = true;
                NoLeft = true;
                NoUp = true;
            }
        }

        int dir = 0;
        Random rnd = new Random();
        private async void GameLoop(object sender, EventArgs e)
        {

            Playerhitboxleft = new Rect(Canvas.GetLeft(Player) - 5, Canvas.GetTop(Player), 10, 10);
            Playerhitboxdown = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player) + 5, 10, 10);
            Playerhitboxup = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player) - 5, 10, 10);
            Playerhitboxright = new Rect(Canvas.GetLeft(Player) + 5, Canvas.GetTop(Player), 10, 10);

            foreach (var x in GameCanvas.Children.OfType<Rectangle>())
            {

                if ((string)x.Tag == "wall")
                {
                    Rect hitWall = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Playerhitboxleft.IntersectsWith(hitWall))
                    {
                        MoveLeft = false;
                    }
                    if (Playerhitboxright.IntersectsWith(hitWall))
                    {
                        MoveRight = false;
                    }
                    if (Playerhitboxup.IntersectsWith(hitWall))
                    {
                        MoveUp = false;
                    }
                    if (Playerhitboxdown.IntersectsWith(hitWall))
                    {
                        MoveDown = false;
                    }
                }
                if ((string)x.Tag == "breakablewall")
                {
                    Rect hitbreakableWall = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Playerhitboxleft.IntersectsWith(hitbreakableWall))
                    {
                        MoveLeft = false;
                    }
                    if (Playerhitboxright.IntersectsWith(hitbreakableWall))
                    {
                        MoveRight = false;
                    }
                    if (Playerhitboxup.IntersectsWith(hitbreakableWall))
                    {
                        MoveUp = false;
                    }
                    if (Playerhitboxdown.IntersectsWith(hitbreakableWall))
                    {
                        MoveDown = false;
                    }
                }
                if ((string)x.Tag == "bomb")
                {
                    Rect hitbomb = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    
                        
                            //Rect hitplayer = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (hitbomb.IntersectsWith(Playerhitboxleft) ||
                               hitbomb.IntersectsWith(Playerhitboxright) ||
                               hitbomb.IntersectsWith(Playerhitboxup) ||
                               hitbomb.IntersectsWith(Playerhitboxdown))
                            {
                                plantedbomb = true;
                                x.Tag = "plantedbomb";
                            }
                        
                    
                    foreach (var y in GameCanvas.Children.OfType<Rectangle>())
                    {
                        if ((string)y.Tag == "blast")
                        {
                            Bombexplode(Canvas.GetLeft(x), Canvas.GetTop(x));
                            blasting(Canvas.GetLeft(x), Canvas.GetTop(x));
                            await Task.Delay(1000);
                            blastremove(Canvas.GetLeft(x), Canvas.GetTop(x));
                        }
                    }
                }
                if ((string)x.Tag == "plantedbomb" || plantedbomb == true)
                {
                    Rect hitplantedbomb = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Playerhitboxleft.IntersectsWith(hitplantedbomb))
                    {
                        MoveLeft = false;
                    }
                    if (Playerhitboxright.IntersectsWith(hitplantedbomb))
                    {
                        MoveRight = false;
                    }
                    if (Playerhitboxup.IntersectsWith(hitplantedbomb))
                    {
                        MoveUp = false;
                    }
                    if (Playerhitboxdown.IntersectsWith(hitplantedbomb))
                    {
                        MoveDown = false;
                    }
                    foreach (var y in GameCanvas.Children.OfType<Rectangle>())
                    {
                        if ((string)y.Tag == "blast")
                        {
                            Bombexplode(Canvas.GetLeft(x), Canvas.GetTop(x));
                            blasting(Canvas.GetLeft(x), Canvas.GetTop(x));
                            await Task.Delay(1000);
                            blastremove(Canvas.GetLeft(x), Canvas.GetTop(x));
                        }
                    }
                }
                if ((string)x.Tag == "enemy")
                {
                    if (dir % 2 == 0)
                        Canvas.SetTop(x, Canvas.GetTop(x) + 2 * enemeymovement);
                    else
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + 2 * enemeymovement);
                    Rect hitenemy = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    Rect hitenemy1 = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x) - 20, x.Width, x.Height + 40);
                    foreach (var y in GameCanvas.Children.OfType<Rectangle>())
                    {
                        if ((string)y.Tag == "breakablewall" || (string)y.Tag == "wall")
                        {
                            Rect hitbreakableWall = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (hitbreakableWall.IntersectsWith(hitenemy))
                            {
                                if (dir % 2 == 0)
                                    Canvas.SetTop(x, Canvas.GetTop(x) - 2 * enemeymovement);
                                else
                                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 2 * enemeymovement);
                                enemeymovement = enemeymovement * -1;
                                dir = rnd.Next(0, 2);
                            }
                        }
                        if ((string)y.Tag == "blast")
                        {
                            Rect hitblast = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (hitblast.IntersectsWith(hitenemy1))
                            {
                                enemeymovement = enemeymovement * -1;
                            }
                        }
                    }
                }
                if ((string)x.Tag == "player")
                {
                    Rect hitplayer = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    foreach (var y in GameCanvas.Children.OfType<Rectangle>())
                    {
                        //player hit blast
                        if ((string)y.Tag == "blast")
                        {
                            Rect hitblast = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (hitblast.IntersectsWith(hitplayer))
                            {
                                playerLife -= 20;
                                Life.Content = playerLife;
                            }
                        }
                        //player hit enemy
                        if ((string)y.Tag == "enemy")
                        {
                            Rect hitenemy = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (hitenemy.IntersectsWith(hitplayer))
                            {
                                playerLife -= 20;
                                Life.Content = playerLife;
                            }
                        }
                        //player hit key
                        if ((string)y.Tag == "key")
                        {
                            Rect hitkey = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (hitkey.IntersectsWith(hitplayer))
                            {
                                playerWithKey = true;
                                ItemsToRemove.Add(y);
                            }
                        }
                        //player hit door
                        if ((string)y.Tag == "door")
                        {
                            Rect hitdoor = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (hitdoor.IntersectsWith(hitplayer))
                            {
                                if (playerWithKey)
                                {
                                    level++;
                                    playerWithKey = false;
                                    blastingpower = 1;
                                    bombmaximum = 1;
                                    isGeneratedDoor = false;
                                    isGeneratedKey = false;
                                    GameCanvas.Children.Clear();
                                    MapSetup();
                                    //Game Go to Next Level
                                }
                            }
                        }
                        //player hit bombpowerup
                        if ((string)y.Tag == "bombpowerup")
                        {
                            Rect hitbombpowerup = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (hitbombpowerup.IntersectsWith(hitplayer))
                            {
                                bombmaximum++;
                                ItemsToRemove.Add(y);
                            }
                        }
                        if ((string)y.Tag == "blastpowerup")
                        {
                            Rect hitblastpowerup = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (hitblastpowerup.IntersectsWith(hitplayer))
                            {
                                blastingpower++;
                                ItemsToRemove.Add(y);
                            }
                        }
                    }
                }
            }

            if (MoveRight == true)
            {
                Canvas.SetLeft(Player, Canvas.GetLeft(Player) + 20);
                //map[playery, playerx] = ' ';
                //map[playery, playerx + 1] = 'P';
            }
            if (MoveDown == true)
            {
                Canvas.SetTop(Player, Canvas.GetTop(Player) + 20);
                //map[playery, playerx] = ' ';
                //map[playery + 1, playerx] = 'P';
            }
            if (MoveUp == true)
            {
                Canvas.SetTop(Player, Canvas.GetTop(Player) - 20);
                //map[playery, playerx] = ' ';
                //map[playery - 1, playerx] = 'P';
            }
            if (MoveLeft == true)
            {
                Canvas.SetLeft(Player, Canvas.GetLeft(Player) - 20);
                //map[playery, playerx] = ' ';
                //map[playery, playerx - 1] = 'P';
            }
            //position.Content = "Left: " + Canvas.GetLeft(Player) + " Top: " + Canvas.GetTop(Player);
            /*if (togglebomb)
            {
                Bombexplode();
            }
            if (toggleblast == false)
            {
                blastremove();
            }*/

            foreach (Rectangle x in ItemsToRemove)
            {
                GameCanvas.Children.Remove(x);
                //please add the following code, after change bomb to rectangle
                /*if (toggleblast)
                {
                    if((string)x.Tag == "Bomb")
                        passbomb(x);
                }*/
            }
            foreach (Ellipse y in EllipsesToRemove)
            {
                GameCanvas.Children.Remove(y);
                //please remove the following code, after change bomb to rectangle
                /*if (toggleblast)
                {
                    passbomb(y);
                }*/
            }

        }

    }
}