using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fillwords.Desktop
{
    class Cell
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public char Letter { get; private set; }
        private int color;
        public int Color {
            get 
            {
                return color;
            }
            set 
            {
                color = Color;
                pCube.Fill = ColorsSet.ColorsList[color, 0];
                tbLetter.Foreground = ColorsSet.ColorsList[color, 1];
            } 
        }
        private Polygon pCube;
        private Label tbLetter;
        public Cell(int x, int y, char letter, int color, Canvas canvas)
        {
            X = x;
            Y = y;
            Letter = letter;
            this.color = color;

            pCube = new Polygon();
            pCube.Width = Settings.CellSize;
            pCube.Height = Settings.CellSize;
            pCube.Fill = ColorsSet.ColorsList[color, 0];

            int cubeX = X * Settings.CellSize + X * 5 + 5;
            int cubeY = Y * Settings.CellSize + Y * 5 + 5;

            pCube.Points = new List<Point>();
            pCube.Points.Add(new Point(cubeX + Settings.CellSize, cubeY));
            pCube.Points.Add(new Point(cubeX, cubeY));
            pCube.Points.Add(new Point(cubeX, cubeY + Settings.CellSize));
            pCube.Points.Add(new Point(cubeX + Settings.CellSize, cubeY + Settings.CellSize));

            tbLetter = new Label();
            tbLetter.Content = Letter.ToString();
            tbLetter.FontSize = Settings.CellSize * 0.9;
            tbLetter.Foreground = ColorsSet.ColorsList[color, 1];
            tbLetter.HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            Canvas.SetLeft(tbLetter, cubeX + Settings.CellSize * 0.15);
            Canvas.SetTop(tbLetter, cubeY - Settings.CellSize * 0.2);

            canvas.Children.Add(pCube);
            canvas.Children.Add(tbLetter);
        }
    }

    //Пометка: у параметров настроек пока не добавлен функционал по их изменению
    abstract class SettingsItem
    {
        public SettingsItemType Type { get; private set; }
        private TextBlock textBlock;
        private Button leftButton;
        private Button rightButton;

        public SettingsItem(int row, string text, Grid grid)
        {
            textBlock = new TextBlock();
            textBlock.Text = text;
            Grid.SetRow(textBlock, row);
            Grid.SetColumn(textBlock, 0);
            grid.Children.Add(textBlock);

            leftButton = new Button();
            leftButton.Content = "<";
            Grid.SetRow(leftButton, row);
            Grid.SetColumn(leftButton, 1);
            grid.Children.Add(leftButton);

            rightButton = new Button();
            rightButton.Content = ">";
            Grid.SetRow(rightButton, row);
            Grid.SetColumn(rightButton, 3);
            grid.Children.Add(rightButton);
        }
    }

    class DigitSettingsItem : SettingsItem
    {
        private TextBox textBox;
        public DigitSettingsItem(int row, string text, Grid grid) : base(row, text, grid)
        {
            textBox = new TextBox();
            textBox.Text = Settings.Property[row].ToString();
            Grid.SetRow(textBox, row);
            Grid.SetColumn(textBox, 2);
            grid.Children.Add(textBox);
        }
    }

    class ColorSettingsItem : SettingsItem
    {
        private TextBlock colorBox;
        public ColorSettingsItem(int row, string text, Grid grid) : base(row, text, grid)
        {
            colorBox = new TextBlock();
            colorBox.Text = "A";
            colorBox.FontSize = 25;
            colorBox.TextAlignment = Avalonia.Media.TextAlignment.Center;
            colorBox.Foreground = ColorsSet.ColorsList[(int)Settings.Property[row], 1];
            colorBox.Background = ColorsSet.ColorsList[(int)Settings.Property[row], 0];
            Grid.SetRow(colorBox, row);
            Grid.SetColumn(colorBox, 2);
            grid.Children.Add(colorBox);
        }
    }

    enum SettingsItemType
    {
        Digit,
        Color
    }
}
