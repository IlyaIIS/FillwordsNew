using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using System;
using System.Collections.Generic;

namespace Fillwords.Desktop
{
    public class Cell
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int CubeX { get; private set; }
        public int CubeY { get; private set; }
        public char Letter { get; private set; }
        private int color;
        public int Color {
            get 
            {
                return color;
            }
            set 
            {
                color = value;
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
            pCube.Tag = this;
            pCube.Width = Settings.CellSize;
            pCube.Height = Settings.CellSize;
            pCube.Fill = ColorsSet.ColorsList[color, 0];

            CubeX = X * Settings.CellSize + X * 5 + 5;
            CubeY = Y * Settings.CellSize + Y * 5 + 5;

            pCube.Points = new List<Point>();
            pCube.Points.Add(new Point(CubeX + Settings.CellSize, CubeY));
            pCube.Points.Add(new Point(CubeX, CubeY));
            pCube.Points.Add(new Point(CubeX, CubeY + Settings.CellSize));
            pCube.Points.Add(new Point(CubeX + Settings.CellSize, CubeY + Settings.CellSize));

            tbLetter = new Label();
            tbLetter.Content = Letter.ToString();
            tbLetter.FontSize = Settings.CellSize * 0.9;
            tbLetter.Foreground = ColorsSet.ColorsList[color, 1];
            tbLetter.HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            Canvas.SetLeft(tbLetter, CubeX + Settings.CellSize * 0.15);
            Canvas.SetTop(tbLetter, CubeY - Settings.CellSize * 0.2);

            canvas.Children.Add(pCube);
            canvas.Children.Add(tbLetter);
        }
    }

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
            leftButton.Tag = row;
            leftButton.Click += LeftOrRightButton_Click;
            Grid.SetRow(leftButton, row);
            Grid.SetColumn(leftButton, 1);
            grid.Children.Add(leftButton);

            rightButton = new Button();
            rightButton.Content = ">";
            rightButton.Tag = row;
            rightButton.Click += LeftOrRightButton_Click;
            Grid.SetRow(rightButton, row);
            Grid.SetColumn(rightButton, 3);
            grid.Children.Add(rightButton);
        }

        private void LeftOrRightButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int propertyNum = (int)button.Tag;
            if (button.Content == "<")
            {
                Settings.Property[propertyNum] = (int)Settings.Property[propertyNum] - 1;
            }
            else
            {
                Settings.Property[propertyNum] = (int)Settings.Property[propertyNum] + 1;
            }
            UpdateItemsContext((Grid)button.Parent);
        }

        public static void UpdateItemsContext(Grid grid)
        {
            foreach (IControl control in grid.Children)
            {
                if (control is TextBox)
                {
                    TextBox tb = (TextBox)control;
                    tb.Text = Settings.Property[(int)tb.Tag].ToString();
                }
                else
                if (control is TextBlock)
                {
                    TextBlock tb = (TextBlock)control;
                    if (tb.Tag != null)
                    {
                        ((TextBlock)control).Foreground = ColorsSet.ColorsList[(int)Settings.Property[(int)tb.Tag], 1];
                        ((TextBlock)control).Background = ColorsSet.ColorsList[(int)Settings.Property[(int)tb.Tag], 0];
                    }
                }
            }
        }
    }

    class DigitSettingsItem : SettingsItem
    {
        private TextBox textBox;
        public DigitSettingsItem(int row, string text, Grid grid) : base(row, text, grid)
        {
            textBox = new TextBox();
            textBox.Text = Settings.Property[row].ToString();
            textBox.Tag = row;
            textBox.LostFocus += TextBox_LostFocus;
            Grid.SetRow(textBox, row);
            Grid.SetColumn(textBox, 2);
            grid.Children.Add(textBox);
        }

        private void TextBox_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (Int32.TryParse(tb.Text, out int result))
                Settings.Property[(int)tb.Tag] = result;
            tb.Text = Settings.Property[(int)tb.Tag].ToString();
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
            colorBox.Tag = row;
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
