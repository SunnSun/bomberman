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
    //yoyo
    public partial class MainWindow
    {
        //testing
        //testing message 2
        public uint GridSize = 20; // size of the food and snake's body
        public uint XGridNum, YGridNum;
        Brush BodyBrush = new SolidColorBrush(Colors.Black);
        Brush FoodBrush = new SolidColorBrush(Colors.Red);
        Random rand = new Random();
        List<Rectangle> ItemsToRemove = new List<Rectangle>();
        List<Ellipse> EllipsesToRemove = new List<Ellipse>();

        public void MapSetup()
        {
            XGridNum = (uint)(GameCanvas.Width / GridSize);
            YGridNum = (uint)(GameCanvas.Height / GridSize);
            double InitX = XGridNum / 2;
            double InitY = YGridNum / 2;
            int playerx;
            int playery;
            randomwallclear();

            string[] map = new string[25];
            map[0] = "wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww";
            map[1] = "w                                w";
            map[2] = "w   w           w          w     w";
            map[3] = "w      w                         w";
            map[4] = "w   w                            w";
            map[5] = "w              w    w            w";
            map[6] = "w        w              w        w";
            map[7] = "w               w                w";
            map[8] = "w    w                w          w";
            map[9] = "w             w          w       w";
            map[10] = "w   w                            w";
            map[11] = "w           w           w        w";
            map[12] = "w     w                          w";
            map[13] = "w        w     w        w        w";
            map[14] = "www                            www";
            map[15] = "w       w      P       w         w";
            map[16] = "w    w      w              w     w";
            map[17] = "w       w              w         w";
            map[18] = "w    w          w          w     w";
            map[19] = "w   w                 w      w   w";
            map[20] = "w       w        w        w      w";
            map[21] = "w    w       w        w          w";
            map[22] = "w        w         w       w     w";
            map[23] = "w    w        w        w         w";
            map[24] = "wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww";

            for (int j = 0; j < 25; j++)
                {
                    for (int i = 0; i < 34; i++)
                    {
                        if (map[j].ToCharArray()[i] == 'w')
                        {
                            Rectangle wall = new Rectangle
                            {
                                Tag = "wall",
                                Height = GridSize,
                                Width = GridSize,
                                Fill = BodyBrush
                            };
                            Canvas.SetLeft(wall, i * GridSize);
                            Canvas.SetTop(wall, j * GridSize);
                            Panel.SetZIndex(wall, 1);
                            GameCanvas.Children.Add(wall);
                        }
                        if (map[j].ToCharArray()[i] == 'P')
                        {
                            Player.Width = 10;
                            Player.Height = 18;
                            playerx = i;
                            playery = j;
                            Canvas.SetLeft(Player, i * GridSize + 5);
                            Canvas.SetTop(Player, j * GridSize + 1);
                        }
                    }
                }
            }
        public void randomwallgenerate()
        {
            for (int i = 0; i < 100; i++)
            {
                Point location = new Point();
                bool IsValidLocation = false;

                while (!IsValidLocation)
                {
                    location.X = rand.Next(0, (int)XGridNum) * GridSize;
                    location.Y = rand.Next(0, (int)YGridNum) * GridSize;

                    IsValidLocation = true;
                    foreach (Rectangle x in GameCanvas.Children)
                    {
                        Point GridLocation = x.TransformToAncestor(GameCanvas).Transform(new Point(0, 0));
                        if (Math.Abs(location.X - GridLocation.X) <= GridSize && Math.Abs(location.Y - GridLocation.Y) <= GridSize)
                        {
                            IsValidLocation = false;
                            break;
                        }
                    }
                }
                Rectangle breakablewall = new Rectangle
                {
                    Tag = "breakablewall",
                    Height = GridSize,
                    Width = GridSize,
                    Fill = FoodBrush
                };

                Canvas.SetLeft(breakablewall, location.X);
                Canvas.SetTop(breakablewall, location.Y);

                GameCanvas.Children.Add(breakablewall);
            }
            
        }
        public void randomwallclear()
        {
            foreach (Rectangle x in GameCanvas.Children)
            {
                if ((string)x.Tag == "breakablewall")
                {
                    ItemsToRemove.Add(x);
                }
            }
        }
    }
}