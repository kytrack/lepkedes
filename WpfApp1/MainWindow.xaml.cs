using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        const int sorokszama = 15;
        const int oszlopokszama = 10;
        const int babukszama = 10;
        const int lekekszama = 10;
        Random vel = new Random();
        List<Point> uresPoziciok = new List<Point>();
        List<Label> babuk = new List<Label>();

        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < sorokszama; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < oszlopokszama; i++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < lekekszama; i++)
            {
                int sor = vel.Next(sorokszama);
                int oszlop = vel.Next(oszlopokszama);

                uresPoziciok.Add(new Point(sor, oszlop));

                Ellipse ujAkna = new Ellipse();
                ujAkna.Width = 30;
                ujAkna.Height = 30;
                ujAkna.Fill = new SolidColorBrush(Colors.Chocolate);

                Grid.SetRow(ujAkna, sor);
                Grid.SetColumn(ujAkna, oszlop);
                GameGrid.Children.Add(ujAkna);
            }

            for (int i = 0; i < sorokszama; i++)
            {
                for (int j = 0; j < oszlopokszama; j++)
                {
                    Border border = new Border();
                    border.BorderBrush = Brushes.Black;
                    border.BorderThickness = new Thickness(1);

                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);

                    GameGrid.Children.Add(border);
                }
            }

            for (int i = 0; i < babukszama; i++)
            {
                int sor = vel.Next(sorokszama);
                int oszlop = vel.Next(oszlopokszama);

                uresPoziciok.Add(new Point(sor, oszlop));

                Label babu = new Label();
                babu.Background = new SolidColorBrush(Colors.LightBlue);
                babu.Content = (i + 1).ToString();
                babu.HorizontalAlignment = HorizontalAlignment.Center;
                babu.VerticalAlignment = VerticalAlignment.Center;

                Grid.SetRow(babu, sor);
                Grid.SetColumn(babu, oszlop);
                GameGrid.Children.Add(babu);
                babuk.Add(babu);
            }
        }

        private void Lepes_Click(object sender, RoutedEventArgs e)
        {
            foreach (var babu in babuk)
            {
                bool lephet = false;

                while (!lephet)
                {
                    int lepesX = 0;
                    int lepesY = 0;

                    int irany = vel.Next(4);
                    switch (irany)
                    {
                        case 0:
                            lepesX = 1; // Jobbra
                            break;
                        case 1:
                            lepesX = -1; // Balra
                            break;
                        case 2:
                            lepesY = 1; // Le
                            break;
                        case 3:
                            lepesY = -1; // Fel
                            break;
                    }

                    int ujSor = Grid.GetRow(babu) + lepesY;
                    int ujOszlop = Grid.GetColumn(babu) + lepesX;

                    if (ujSor >= 0 && ujSor < sorokszama && ujOszlop >= 0 && ujOszlop < oszlopokszama &&
                        !babuk.Any(b => Grid.GetRow(b) == ujSor && Grid.GetColumn(b) == ujOszlop))
                    {
                        if (uresPoziciok.Contains(new Point(ujSor, ujOszlop)))
                        {
                            var lyuk = GameGrid.Children.OfType<Ellipse>().FirstOrDefault(el =>
                                Grid.GetRow(el) == ujSor && Grid.GetColumn(el) == ujOszlop);
                            if (lyuk != null)
                                GameGrid.Children.Remove(babu);
                        }

                        Grid.SetRow(babu, ujSor);
                        Grid.SetColumn(babu, ujOszlop);
                        lephet = true;
                    }
                }
            }
        }
    }
}
