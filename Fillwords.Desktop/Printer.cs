using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fillwords.Desktop
{
    static public class Printer
    {
        static public Controls StackPanelItemList { get; private set; }
        static public Window MainWindow { get; private set; }
        static public Window CurrentWindow { get; private set; }
        delegate void EventDel(object sender, RoutedEventArgs e);

        static Field field;
        static public void SetPrinterParams(Controls stackPanelItemList, Window mainWindow)
        {
            StackPanelItemList = stackPanelItemList;
            MainWindow = mainWindow;
            CurrentWindow = mainWindow;
        }

        static public void SetMainWindow()
        {
            if (MainWindow != CurrentWindow)
            {
                CurrentWindow.Close();
                MainWindow.Show();
                CurrentWindow = MainWindow;
                return;
            }

            int buttonsFontSize = 40;
            int buttonsWidth = 320;

            StackPanelItemList.Clear();

            StackPanelItemList.Add(CreateTitleTextBlock("FILLWORDS", buttonsFontSize * 3));

            StackPanelItemList.Add(CreateButton("НОВАЯ ИГРА", ItemEvents.btnStartNewGame_Click, buttonsFontSize, buttonsWidth));
            StackPanelItemList.Add(CreateButton("ПРОДОЛЖИТЬ", ItemEvents.btnContinue_Click, buttonsFontSize, buttonsWidth));
            StackPanelItemList.Add(CreateButton("РЕЙТИНГ",    ItemEvents.btnRecords_Click,  buttonsFontSize, buttonsWidth));
            StackPanelItemList.Add(CreateButton("НАСТРОЙКИ",  ItemEvents.btnSettings_Click, buttonsFontSize, buttonsWidth));
            StackPanelItemList.Add(CreateButton("ВЫХОД",      ItemEvents.btnMainExit_Click, buttonsFontSize, buttonsWidth));
        }

        static private Button CreateButton(string text, EventDel eventClick, int fontSize = 20, int width = 0, 
            Avalonia.Layout.HorizontalAlignment horizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center)
        {
            var button = new Button();

            button.Content = text;
            button.FontSize = fontSize;
            if (width != 0) button.Width = width;
            button.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(eventClick);
            button.HorizontalAlignment = horizontalAlignment;

            button.HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            
            return button;
        }

        static private TextBlock CreateTitleTextBlock(string text, int fontSize)
        {
            var tbTitle = new TextBlock();
            tbTitle.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            tbTitle.FontSize = fontSize;
            tbTitle.FontStyle = Avalonia.Media.FontStyle.Italic;
            tbTitle.Text = "FILLWORDS";
            return tbTitle;
        }

        static public void SetErrorWindow(string text)
        {
            var gameWin = CreateWindow(MainWindow.Width, MainWindow.Height);
            gameWin.Show();
            CurrentWindow = gameWin;

            gameWin.Content = new StackPanel();

            var tbErrorText = new TextBlock();
            tbErrorText.Text = text;
            tbErrorText.FontSize = 20;
            tbErrorText.Margin = new Thickness((int)(MainWindow.Height / 4));
            tbErrorText.TextWrapping = TextWrapping.Wrap;
            tbErrorText.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            tbErrorText.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;

            ((StackPanel)gameWin.Content).Children.Add(tbErrorText);
        }

        static public void SetRecordsWindow()
        {
            var gameWin = CreateWindow(MainWindow.Width, MainWindow.Height);
            MainWindow.Hide();
            gameWin.Show();
            CurrentWindow = gameWin;

            StackPanel stackPanel = new StackPanel();
            gameWin.Content = stackPanel;

            stackPanel.Children.Add(CreateButton("Назад", ItemEvents.btnExitToMainWindow_Click, horizontalAlignment : Avalonia.Layout.HorizontalAlignment.Right));

            stackPanel.Children.Add(CreateTitleTextBlock("РЕКОРДЫ", 50));

            foreach (var user in DataWorker.UserScoreDict)
                stackPanel.Children.Add(CreateTextBlock(user.Key + ": " + user.Value));
        }

        static private TextBlock CreateTextBlock(string text)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;

            return textBlock;
        }

        static public void SetSettingsWindow()
        {
            var gameWin = new Window();
            gameWin.Width = MainWindow.Width;
            gameWin.Height = MainWindow.Height;
            gameWin.Closed += new EventHandler(ItemEvents.wCloseSettingsWindow);
            MainWindow.Hide();
            gameWin.Show();
            CurrentWindow = gameWin;

            StackPanel stackPanel = new StackPanel();
            gameWin.Content = stackPanel;

            stackPanel.Children.Add(CreateButton("Назад", ItemEvents.wCloseSettingsWindow, horizontalAlignment: Avalonia.Layout.HorizontalAlignment.Right));

            stackPanel.Children.Add(CreateTitleTextBlock("НАСТРОЙКИ", 50));

            Grid grid = new Grid();
            SetDefinitionToGrid(grid, 4, 11);
            stackPanel.Children.Add(grid);

            CreateSettingsItem(0, "Ширина поля", grid, SettingsItemType.Digit);
            CreateSettingsItem(1, "Высота поля", grid, SettingsItemType.Digit);
            CreateSettingsItem(2, "Размер ячейки", grid, SettingsItemType.Digit);
            CreateSettingsItem(3, "Цвет поля", grid, SettingsItemType.Color);
            CreateSettingsItem(4, "Цвет текущей ячейки под курсором", grid, SettingsItemType.Color);
            CreateSettingsItem(5, "Цвет выделенного слова", grid, SettingsItemType.Color);
            CreateSettingsItem(6, "Цвет отгаданных слов", grid, SettingsItemType.Color);

            var tbRandomButton = new TextBlock() { Text = "Случайный цвет отгаданных слов" };
            Grid.SetRow(tbRandomButton, 7);
            Grid.SetColumn(tbRandomButton, 0);
            grid.Children.Add(tbRandomButton);
            var bRandom = new Button();
            bRandom.Content = Settings.Property[7].ToString();
            bRandom.Click += BRandom_Click;
            Grid.SetRow(bRandom, 7);
            Grid.SetColumn(bRandom, 2);
            grid.Children.Add(bRandom);

            var bReset = new Button() { Content = "Установить настройки по умолчанию" };
            bReset.Click += BReset_Click;
            Grid.SetRow(bReset, 8);
            Grid.SetColumn(bReset, 0);
            grid.Children.Add(bReset);
        }

        private static void BReset_Click(object? sender, RoutedEventArgs e)
        {
            Settings.SetDefaultSettings();
            SettingsItem.UpdateItemsContext((Grid)((Button)sender).Parent);
        }

        private static void BRandom_Click(object? sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Settings.Property[7] = !(button.Content == "True");
            button.Content = Settings.Property[7].ToString();
        }

        static public void SetSettingsWindowOld()
        {
            var settingsPropertyArr = new string[] 
            { 
                "Ширина поля",
                "Высота поля",
                "Размер ячейки",
                "Цвет поля",
                "Цвет текущей ячейки под курсором",
                "Цвет выделенного слова",
                "Цвет отгаданных слов",
                "Случайный цвет отгаданных слов",
                "Установить настройки по умолчанию" 
            };

            var gameWin = CreateWindow(MainWindow.Width, MainWindow.Height);
            MainWindow.Hide();
            gameWin.Show();
            CurrentWindow = gameWin;

            StackPanel stackPanel = new StackPanel();
            gameWin.Content = stackPanel;

            stackPanel.Children.Add(CreateButton("Назад", ItemEvents.btnExitToMainWindow_Click, horizontalAlignment: Avalonia.Layout.HorizontalAlignment.Right));

            stackPanel.Children.Add(CreateTitleTextBlock("НАСТРОЙКИ", 50));

            for (int i = 0; i < settingsPropertyArr.Length; i++)
            {
                stackPanel.Children.Add(CreateSettingsDockPanel(settingsPropertyArr[i], Settings.Property[i].ToString()));
            }
        }

        static private DockPanel CreateSettingsDockPanel(string text, string score)
        {
            DockPanel dockPanel = new DockPanel();

            var textBlock = new TextBlock();
            textBlock.Text = text;

            var btnLeft = new Button();
            btnLeft.Content = "<";

            var btnRight = new Button();
            btnRight.Content = ">";

            var tbScore = new TextBox();
            tbScore.Text = score;

            dockPanel.Children.Add(textBlock);
            dockPanel.Children.Add(btnLeft);
            dockPanel.Children.Add(tbScore);
            dockPanel.Children.Add(btnRight);

            dockPanel.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right;

            return dockPanel;
        }

        static private SettingsItem CreateSettingsItem(int row, string text, Grid grid, SettingsItemType type)
        {
            if (type == SettingsItemType.Digit)
                return new DigitSettingsItem(row, text, grid);
            else
                return new ColorSettingsItem(row, text, grid);
        }

        static public void SetNewGameWindowStart()
        {
            var gameWin = CreateWindow(MainWindow.Width, MainWindow.Height);
            MainWindow.Hide();
            gameWin.Show();
            CurrentWindow = gameWin;

            var grid = new Grid();
            gameWin.Content = grid;

            var tb = new TextBox();
            tb.KeyUp += new EventHandler<Avalonia.Input.KeyEventArgs>(ItemEvents.tbEnterEnter);

            grid.Children.Add(new TextBlock() { Text = "Введите имя: " });
            grid.Children.Add(tb);
        }
        static private Window CreateWindow(double width, double height)
        {
            var window = new Window();
            window.Width = width;
            window.Height = height;
            window.Closed += new EventHandler(ItemEvents.wCloseAdditionalWindow);

            return window;
        }

        static public void SetNewGameWindowNext()
        {
            field = new Field();
            field.CreateNewField(Settings.XSize, Settings.YSize, DataWorker.WordsSet);

            var grid = (Grid)CurrentWindow.Content;

            grid.Children.Clear();
            SetDefinitionToGrid(grid, 2, 1);

            var canvas = new Canvas();
            grid.Children.Add(canvas);
            Grid.SetColumn(canvas, 0);
            Grid.SetRow(canvas, 0);

            SetFieldOnCanvas(canvas);
        }

        static private void SetDefinitionToGrid(Grid grid, int x, int y)
        {
            for (int ii = 0; ii < x; ii++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto});
            }
            for (int i = 0; i < y; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }
        }

        static private void SetFieldOnCanvas(Canvas canvas)
        {
            for (int y = 0; y < field.YSize; y++)
            {
                for (int x = 0; x < field.XSize; x++)
                {
                    Cell cell = new Cell(x, y, field.CellLetter[x, y], field.CellColor[x, y], canvas);
                }
            }
            
        }
    }
}
