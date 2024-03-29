﻿using System;
using System.IO;
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
    //B = breakableWall, w = Wall, P = Player, E = ememy 
    public partial class MainWindow
    {
        public uint GridSize = 20; // size of the food and snake's body
        public uint XGridNum, YGridNum;
        char[,] map = new char[25, 35];
        Brush BodyBrush = new SolidColorBrush(Colors.Black);
        Brush EnemeyBrush = new SolidColorBrush(Colors.DarkRed);
        Brush FoodBrush = new SolidColorBrush(Colors.Red);
        Random rand = new Random();
        List<Rectangle> ItemsToRemove = new List<Rectangle>();
        List<Ellipse> EllipsesToRemove = new List<Ellipse>();
        int playerx;
        int playery;
        int level = 1;
        ImageBrush playersprite = new ImageBrush();
        //playersprite = new BitmapImage(new Uri("pack://application:,,,/image/player.png"));

        Rectangle Player = new Rectangle
        {
            Tag = "player",
            Height = 17,
            Width = 17,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/ierg3080-Bombman;component/player.png", UriKind.Absolute))

            }
            //Fill = Brushes.Yellow,
            //ImageBrush Player = new ImageBrush(),
            //BitmapImage bmi = new BitmapImage(new Uri(@"/WpfImageApp;component/Images/Penguins.jpg", UriKind.Relative));
            //ib.ImageSource = bmi;
            //body.Fill = ib;
        };
        public void MapSetup()
        {
            XGridNum = (uint)(GameCanvas.Width / GridSize);
            YGridNum = (uint)(GameCanvas.Height / GridSize);
            double InitX = XGridNum / 2;
            double InitY = YGridNum / 2;
            Random rnd = new Random();
            
            //randomwallclear();
            for (int row = 0; row < 25; row++)
            {
                for (int col = 0; col < 35; col++)
                {
                    map[row, col] = ' ';
                }
            }
            map[2, 1] = 'x';
            for (int row = 0; row < 25; row++)
            {
                if (row == 1)
                {
                    map[row, 0] = 'w';
                    map[row, 34] = 'w';
                    map[row, 1] = 'P';
                    map[row, 2] = 'x';
                }
                else if (row % 2 == 1)
                {
                    map[row, 0] = 'w';
                    map[row, 34] = 'w';
                }
                else for (int col = 0; col < 35; col++)
                    {
                        if (row == 0 || row == 24)
                        {
                            map[row, col] = 'w';
                        }
                        if (row % 2 == 0 && col % 2 == 0)
                        {
                            map[row, col] = 'w';
                        }
                        if (col == 34)
                        {
                            map[row, col] = 'w';
                        }
                    }
            }
            int x, y;
            for (int i = 0; i < level * 2; i++)
            {
                do
                {
                    x = rnd.Next(1, 23);
                    y = rnd.Next(1, 33);
                } while (map[x, y] != ' ');
                map[x, y] = 'E';
            }
            do
            {
                x = rnd.Next(1, 23);
                y = rnd.Next(1, 33);
            } while (map[x, y] != ' ');
            map[x, y] = 'D';
            do
            {
                x = rnd.Next(1, 23);
                y = rnd.Next(1, 33);
            } while (map[x, y] != ' ');
            map[x, y] = 'K';
            for (int j = 0; j< 25; j++)
            {
                for(int i = 0; i<35; i++)
                {
                    if (map[j, i] == 'w')
                    {
                        Rectangle wall = new Rectangle
                        {
                            Tag = "wall",
                            Height = GridSize,
                            Width = GridSize,
                            Fill = new ImageBrush
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/ierg3080-Bombman;component/wall.png", UriKind.Absolute))
                            }
                            //Fill = BodyBrush
                        };
                        Canvas.SetLeft(wall, i * GridSize);
                        Canvas.SetTop(wall, j * GridSize);
                        Panel.SetZIndex(wall, 1);
                        GameCanvas.Children.Add(wall);
                    }
                    if (map[j, i] == 'P')
                    {
                        playerx = i;
                        playery = j;
                        GameCanvas.Children.Add(Player);
                        Canvas.SetLeft(Player, i * GridSize+3);
                        Canvas.SetTop(Player, j * GridSize+3);

                    }
                    if (map[j, i] == 'E')
                    {
                        Rectangle enemy = new Rectangle
                        {
                            Tag = "enemy",
                            Height = 15,
                            Width = 15,
                            Fill = new ImageBrush
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/ierg3080-Bombman;component/enemy.png", UriKind.Absolute))
                            }
                            //Fill = EnemeyBrush
                        };
                        Canvas.SetLeft(enemy, i * GridSize + 1);
                        Canvas.SetTop(enemy, j * GridSize + 1);
                        Panel.SetZIndex(enemy, 1);
                        GameCanvas.Children.Add(enemy);
                    }
                    if (map[j, i] == 'D')
                    {
                        Rectangle Door = new Rectangle
                        {
                            Tag = "breakablewall",
                            Height = 20,
                            Width = 20,
                            Fill = new ImageBrush
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/ierg3080-Bombman;component/breakablewall.png", UriKind.Absolute))
                            }
                            //Fill = EnemeyBrush
                        };
                        Canvas.SetLeft(Door, i * GridSize + 1);
                        Canvas.SetTop(Door, j * GridSize + 1);
                        Panel.SetZIndex(Door, 1);
                        GameCanvas.Children.Add(Door);
                    }
                    if (map[j, i] == 'K')
                    {
                        Rectangle Key = new Rectangle
                        {
                            Tag = "breakablewall",
                            Height = 20,
                            Width = 20,
                            Fill = new ImageBrush
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/ierg3080-Bombman;component/breakablewall.png", UriKind.Absolute))
                            }
                            //Fill = EnemeyBrush
                        };
                        Canvas.SetLeft(Key, i * GridSize + 1);
                        Canvas.SetTop(Key, j * GridSize + 1);
                        Panel.SetZIndex(Key, 1);
                        GameCanvas.Children.Add(Key);
                    }
                }
            }

            /*
            map[0] = "wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww";
            map[1] = "wP                               w";
            map[2] = "w w w w w w w w w w w w w w w w ww";
            map[3] = "w                                w";
            map[4] = "w w w w w w w w w w w w w w w w ww";
            map[5] = "w                                w";
            map[6] = "w w w w w w w w w w w w w w w w ww";
            map[7] = "w                                w";
            map[8] = "w w w w w w w w w w w w w w w w ww";
            map[9] = "w                                w";
            map[10] = "w w w w w w w w w w w w w w w w ww";
            map[11] = "w                                w";
            map[12] = "w w w w w w w w w w w w w w w w ww";
            map[13] = "w                                w";
            map[14] = "w w w w w w w w w w w w w w w w ww";
            map[15] = "w                                w";
            map[16] = "w w w w w w w w w w w w w w w w ww";
            map[17] = "w                                w";
            map[18] = "w w w w w w w w w w w w w w w w ww";
            map[19] = "w                                w";
            map[20] = "w w w w w w w w w w w w w w w w ww";
            map[21] = "w                                w";
            map[22] = "w w w w w w w w w w w w w w w w ww";
            map[23] = "w                                w";
            map[24] = "wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww";*/

            //drawmap(map);
        }

        

        //<Rectangle x:Name="Player" Tag="player" Height="18" Canvas.Left="187" Fill="Yellow" Canvas.Top="159" Width="10"/>
        public void randomwallgenerate()
        {
            int wallNum = rand.Next(level*70, level*110);
            for (int i = 0; i < wallNum; i++)
            {
                Point location = new Point();
                bool IsValidLocation = false;

                while (!IsValidLocation)
                {
                    location.X = rand.Next(1, (int)XGridNum) * GridSize;
                    location.Y = rand.Next(1, (int)YGridNum) * GridSize;
                    /* while (map[(int)location.X][(int)location.Y] != ' ')
                     {
                         location.X = rand.Next(0, (int)XGridNum) * GridSize;
                         location.Y = rand.Next(0, (int)YGridNum) * GridSize;
                     }*/

                    IsValidLocation = true;

                    foreach (Rectangle x in GameCanvas.Children)
                    {
                        Point GridLocation = x.TransformToAncestor(GameCanvas).Transform(new Point(0, 0));
                        if (Math.Abs(location.X - GridLocation.X) <= GridSize && Math.Abs(location.Y - GridLocation.Y) <= GridSize)
                        {
                            IsValidLocation = false;
                            break;
                        }

                        if ((location.X == 40 && location.Y == 20) || (location.X == 20 && location.Y == 40))
                        {
                            IsValidLocation = false;
                            break;
                        }
                        if (location.X == 40 && location.Y == 120)
                        {
                            IsValidLocation = false;
                            break;
                        }
                        if (map[(int)location.Y / 20, (int)location.X / 20] != ' ')
                        {
                            IsValidLocation = false;
                            break;
                        }

                    }
                }
                Rectangle breakablewall = new Rectangle
                {
                    Tag = "breakablewall",
                    Uid = (location.X + location.Y * 35).ToString(),
                    Height = GridSize,
                    Width = GridSize,
                    Fill = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/ierg3080-Bombman;component/breakablewall.png", UriKind.Absolute))
                    }
                    //Fill = Brushes.Gray
                };
                levellabel.Content = "level: " + level;
                map[(int)location.Y / 20, (int)location.X / 20] = 'B';
                Canvas.SetLeft(breakablewall, location.X);
                Canvas.SetTop(breakablewall, location.Y);

                GameCanvas.Children.Add(breakablewall);
                /*StringBuilder sb = new StringBuilder(map[(int)location.X]);
                sb[(int)location.Y] = 'B';
                map[(int)location.X] = sb.ToString();*/
            }

        }
        public void randomwallclear()
        {
            foreach (Rectangle x in GameCanvas.Children)
            {
                if ((string)x.Tag != "wall")
                {
                    ItemsToRemove.Add(x);
                }
            }
        }
    }
}