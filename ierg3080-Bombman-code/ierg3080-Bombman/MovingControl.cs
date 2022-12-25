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
    public partial class MainWindow
    {
        Rect Playerhitboxleft, Playerhitboxright, Playerhitboxup, Playerhitboxdown;
        private void keyreleased(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                MoveUp = false;
            }
            if (e.Key == Key.S)
            {
                MoveDown = false;
            }
            if (e.Key == Key.A)
            {
                MoveLeft = false;
            }
            if (e.Key == Key.D)
            {
                MoveRight = false;
            }
            if (!togglebomb)
            {
                if (e.Key == Key.Space)
                {
                    Ellipse Bomb = new Ellipse
                    {
                        Tag = "Bomb",
                        Height = 10,
                        Width = 10,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        Fill = Brushes.Blue
                    };
                    Canvas.SetTop(Bomb, Canvas.GetTop(Player) + 5);
                    Canvas.SetLeft(Bomb, Canvas.GetLeft(Player));
                    GameCanvas.Children.Add(Bomb);
                    togglebomb = true;
                    CurrentCountdown = SpawnCountdown;
                }
            }

        }

        private void keypressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                MoveUp = true;
            }
            if (e.Key == Key.S)
            {
                MoveDown = true;
            }
            if (e.Key == Key.A)
            {
                MoveLeft = true;
            }
            if (e.Key == Key.D)
            {
                MoveRight = true;
            }
        }

        private void GameLoop(object sender, EventArgs e)
        {
            Playerhitboxleft = new Rect(Canvas.GetLeft(Player) - 5, Canvas.GetTop(Player), Player.Width, Player.Height);
            Playerhitboxdown = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player) + 2, Player.Width, Player.Height);
            Playerhitboxup = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player) - 2, Player.Width, Player.Height);
            Playerhitboxright = new Rect(Canvas.GetLeft(Player) + 5, Canvas.GetTop(Player), Player.Width, Player.Height);

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
            }

            if (MoveRight == true)
            {
                Canvas.SetLeft(Player, Canvas.GetLeft(Player) + 20);
            }
            if (MoveDown == true)
            {
                Canvas.SetTop(Player, Canvas.GetTop(Player) + 20);
            }
            if (MoveUp == true)
            {
                Canvas.SetTop(Player, Canvas.GetTop(Player) - 20);
            }
            if (MoveLeft == true)
            {
                Canvas.SetLeft(Player, Canvas.GetLeft(Player) - 20);
            }

            if (togglebomb)
            {
                Bombexplode();
                toggleblast = true;
                CurrentCountdown2 = SpawnCountdown;
            }
            if(toggleblast)
            {
                blastremove();
            }

            foreach (Rectangle y in ItemsToRemove)
            {
                // remove them permanently from the canvas
                GameCanvas.Children.Remove(y);
            }
            foreach (Ellipse y in EllipsesToRemove)
            {
                blasting(y);
                GameCanvas.Children.Remove(y);
            }
        }

    }
}