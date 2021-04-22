using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Collections.Generic;

namespace Fillwords.Desktop
{
    static public class Printer
    {
        public static Window MainWindow { get; private set; }
        private static Controls stackPanelItemList;
        private static Window currentWindow;
        private static List<Cell> cellsList;
        delegate void EventDel(object sender, RoutedEventArgs e);
        private static Field field;
        public static void SetPrinterParams(Controls _stackPanelItemList, Window mainWindow)
        {
            stackPanelItemList = _stackPanelItemList;
            MainWindow = mainWindow;
            currentWindow = mainWindow;
        }

        public static void SetMainWindow()
        {
            if (MainWindow != currentWindow)
            {
                currentWindow.Close();
                MainWindow.Show();
                currentWindow = MainWindow;
                return;
            }

            int buttonsFontSize = 40;
            int buttonsWidth = 320;

            stackPanelItemList.Clear();

            stackPanelItemList.Add(CreateTitleTextBlock("FILLWORDS", buttonsFontSize * 3));

            stackPanelItemList.Add(CreateButton("НОВАЯ ИГРА", ItemEvents.btnStartNewGame_Click, buttonsFontSize, buttonsWidth));
            stackPanelItemList.Add(CreateButton("ПРОДОЛЖИТЬ", ItemEvents.btnContinue_Click, buttonsFontSize, buttonsWidth));
            stackPanelItemList.Add(CreateButton("РЕЙТИНГ",    ItemEvents.btnRecords_Click,  buttonsFontSize, buttonsWidth));
            stackPanelItemList.Add(CreateButton("НАСТРОЙКИ",  ItemEvents.btnSettings_Click, buttonsFontSize, buttonsWidth));
            stackPanelItemList.Add(CreateButton("ВЫХОД",      ItemEvents.btnMainExit_Click, buttonsFontSize, buttonsWidth));
        }

        private static Button CreateButton(string text, EventDel eventClick, int fontSize = 20, int width = 0, 
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

        private static TextBlock CreateTitleTextBlock(string text, int fontSize)
        {
            var tbTitle = new TextBlock();
            tbTitle.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            tbTitle.FontSize = fontSize;
            tbTitle.FontStyle = Avalonia.Media.FontStyle.Italic;
            tbTitle.Text = "FILLWORDS";
            return tbTitle;
        }

        public static void SetErrorWindow(string text)
        {
            var gameWin = CreateWindow(MainWindow.Width, MainWindow.Height);
            gameWin.Show();
            currentWindow = gameWin;

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

        public static void SetRecordsWindow()
        {
            var gameWin = CreateWindow(MainWindow.Width, MainWindow.Height);
            MainWindow.Hide();
            gameWin.Show();
            currentWindow = gameWin;

            StackPanel stackPanel = new StackPanel();
            gameWin.Content = stackPanel;

            stackPanel.Children.Add(CreateButton("Назад", ItemEvents.btnExitToMainWindow_Click, horizontalAlignment : Avalonia.Layout.HorizontalAlignment.Right));

            stackPanel.Children.Add(CreateTitleTextBlock("РЕКОРДЫ", 50));

            foreach (var user in DataWorker.UserScoreDict)
                stackPanel.Children.Add(CreateTextBlock(user.Key + ": " + user.Value));
        }

        private static TextBlock CreateTextBlock(string text)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;

            return textBlock;
        }

        public static void SetSettingsWindow()
        {
            var gameWin = new Window();
            gameWin.Width = MainWindow.Width;
            gameWin.Height = MainWindow.Height;
            gameWin.Closed += new EventHandler(ItemEvents.wSettingsWindow_Close);
            MainWindow.Hide();
            gameWin.Show();
            currentWindow = gameWin;

            StackPanel stackPanel = new StackPanel();
            gameWin.Content = stackPanel;

            stackPanel.Children.Add(CreateButton("Назад", ItemEvents.wSettingsWindow_Close, horizontalAlignment: Avalonia.Layout.HorizontalAlignment.Right));

            stackPanel.Children.Add(CreateTitleTextBlock("НАСТРОЙКИ", 50));

            Grid grid = new Grid();
            SetDefinitionToGrid(grid, 4, 11);
            stackPanel.Children.Add(grid);

            CreateSettingsItem(0, "Ширина поля", grid, SettingsItemType.Digit);
            CreateSettingsItem(1, "Высота поля", grid, SettingsItemType.Digit);
            CreateSettingsItem(2, "Размер ячейки", grid, SettingsItemType.Digit);
            CreateSettingsItem(3, "Цвет поля", grid, SettingsItemType.Color);
            CreateSettingsItem(4, "Цвет выделенного слова", grid, SettingsItemType.Color);
            CreateSettingsItem(5, "Цвет отгаданных слов", grid, SettingsItemType.Color);

            var tbRandomButton = new TextBlock() { Text = "Случайный цвет отгаданных слов" };
            Grid.SetRow(tbRandomButton, 7);
            Grid.SetColumn(tbRandomButton, 0);
            grid.Children.Add(tbRandomButton);

            var bRandom = new Button();
            bRandom.Content = Settings.Property[7].ToString();
            bRandom.Click += btnRandom_Click;
            Grid.SetRow(bRandom, 7);
            Grid.SetColumn(bRandom, 2);
            grid.Children.Add(bRandom);

            var bReset = new Button() { Content = "Установить настройки по умолчанию" };
            bReset.Click += btnReset_Click;
            Grid.SetRow(bReset, 8);
            Grid.SetColumn(bReset, 0);
            grid.Children.Add(bReset);
        }

        private static void btnReset_Click(object? sender, RoutedEventArgs e)
        {
            Settings.SetDefaultSettings();
            SettingsItem.UpdateItemsContext((Grid)((Button)sender).Parent);
        }
        private static void btnRandom_Click(object? sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Settings.Property[7] = !(button.Content == "True");
            button.Content = Settings.Property[7].ToString();
        }

        private static SettingsItem CreateSettingsItem(int row, string text, Grid grid, SettingsItemType type)
        {
            if (type == SettingsItemType.Digit)
                return new DigitSettingsItem(row, text, grid);
            else
                return new ColorSettingsItem(row, text, grid);
        }

        public static void SetNewGameWindow()
        {
            var gameWin = CreateWindow(MainWindow.Width, MainWindow.Height);
            MainWindow.Hide();
            gameWin.Show();
            currentWindow = gameWin;

            var grid = new Grid();
            gameWin.Content = grid;

            var tb = new TextBox();
            tb.KeyUp += new EventHandler<Avalonia.Input.KeyEventArgs>(ItemEvents.tbEnter_Enter);

            grid.Children.Add(new TextBlock() { Text = "Введите имя: " });
            grid.Children.Add(tb);

            field = new Field();
            field.CreateNewField(Settings.XSize, Settings.YSize, DataWorker.WordsSet);
            Player.CreateNewPlayer();
        }

        // Сохранение игры при закрытии окна
        private static void GameWin_Closed(object? sender, EventArgs e)
        {
            if (Player.WordsList.Count != field.WordsList.Count)
            {
                DataWorker.SaveField(field, "field_save.txt");
                DataWorker.SavePlayer("plyer_save.txt");
            }

            SetMainWindow();
        }

        private static Window CreateWindow(double width, double height)
        {
            var window = new Window();
            window.Width = width;
            window.Height = height;
            window.Closed += new EventHandler(ItemEvents.wAdditionalWindow_Close);

            return window;
        }

        public static void SetGameWindow()
        {
            currentWindow.PointerPressed += GameWin_PointerPressed;
            currentWindow.PointerMoved += GameWin_PointerMoved;
            currentWindow.PointerReleased += GameWin_PointerReleased;
            currentWindow.Closed += GameWin_Closed;

            var grid = (Grid)currentWindow.Content;

            grid.Children.Clear();
            SetDefinitionToGrid(grid, 2, 1);

            var canvas = new Canvas();
            canvas.Width = Settings.XSize * (Settings.CellSize + 5) + 5;
            canvas.Height = Settings.YSize * (Settings.CellSize + 5) + 5;
            canvas.Background = Brushes.LightGray;
            grid.Children.Add(canvas);
            Grid.SetColumn(canvas, 0);
            Grid.SetRow(canvas, 0);

            var spRightPanel = new StackPanel();
            grid.Children.Add(spRightPanel);
            Grid.SetColumn(spRightPanel, 1);
            Grid.SetRow(spRightPanel, 0);

            spRightPanel.Children.Add(CreateTextBlock("Очки: " + Player.Score, 20, 20 * 15, 
                                                      new Thickness(5, 5, 0, 0), Brushes.LightGray));

            spRightPanel.Children.Add(CreateTextBlock(" ", 20, 20 * 15, new Thickness(5, 5, 0, 0), Brushes.LightGray));

            spRightPanel.Children.Add(new StackPanel());

            SetFieldOnCanvas(canvas);
        }
        private static TextBlock CreateTextBlock(string text, int fontSize, int width, Thickness margin, ISolidColorBrush color)
        {
            return new TextBlock()
            {
                Text = text,
                FontSize = fontSize,
                Width = width,
                Margin = margin,
                Background = color
            };
        }

        // Начало выделения слова
        private static void GameWin_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            Cell cell = FindCellByCoords(e.GetPosition((Window)sender));
            if (cell != null && cell.Color == Settings.FieldColor)
            {
                cell.Color = Settings.PickedWordColor;
                MouseInfo.IsPressed = true;
                Player.WordNow += cell.Letter;
                ((TextBlock)((StackPanel)((Grid)currentWindow.Content).Children[1]).Children[1]).Text = Player.WordNow;
                Player.CoordStory = new List<int[]> { new int[] { cell.X, cell.Y } };
            }
        }
        // Процесс выделения слова
        private static void GameWin_PointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Cell cell = FindCellByCoords(e.GetPosition((Window)sender));
            if (cell != null && MouseInfo.IsPressed && cell.Color == Settings.FieldColor)
            {
                cell.Color = Settings.PickedWordColor;
                Player.WordNow += cell.Letter;
                ((TextBlock)((StackPanel)((Grid)currentWindow.Content).Children[1]).Children[1]).Text = Player.WordNow;
                Player.CoordStory.Add(new int[] { cell.X, cell.Y });
            }
        }
        // Процесс выделения закончен
        private static void GameWin_PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
        {
            MouseInfo.IsPressed = false;
            if (Player.WordNow != string.Empty)
            {
                Cell cell = FindCellByCoords(e.GetPosition(currentWindow));
                CheckWordNow((cell != null) ? cell.X : 0);
                SetCellsColorFromField();
                UpdateScore();
                UpdateGuessedWordsList();
                Player.WordNow = string.Empty;
                ((TextBlock)((StackPanel)((Grid)currentWindow.Content).Children[1]).Children[1]).Text = " ";
                CheckWin();
            }
        }

        private static Cell FindCellByCoords(Point coord)
        {
            if (cellsList != null)
                foreach (Cell cell in cellsList)
                {
                    if (cell.CubeX <= coord.X && cell.CubeX + Settings.CellSize >= coord.X)
                        if (cell.CubeY <= coord.Y && cell.CubeY + Settings.CellSize >= coord.Y)
                            return cell;
                }
            return null;
        }

        private static void CheckWordNow(int x)
        {
            if (field.WordsList.Contains(Player.WordNow) &&
                field.WordPos[field.WordsList.IndexOf(Player.WordNow)][Player.WordNow.Length - 1].X == x)
            {
                int color = Settings.IsRandomGuessedWordColro? ColorsSet.GetRandomColor() : Settings.GuessedWordColor;
                LogicMethods.ActionsIfWordSelected(field, color);
            } 
            else
            {
                if (field.WordsList.Contains(Player.WordNow))
                    ShowMessageBox(400, 50, "Попробуйте записать это слово наоборот или найти ещё одно такое же на поле");
                else if ((DataWorker.WordsSet.AllWords as IList<string>).Contains(Player.WordNow))
                    ShowMessageBox(400, 50, "Это не одно из слов, которое вам нужно отгодать на данном поле ):");
                else
                    ShowMessageBox(400, 50, "Такого слова нет в словаре");
            }
        }

        public static void SetContinueWindow()
        {
            if (DataWorker.saveExist("field_save.txt", "plyer_save.txt"))
            {
                var gameWin = CreateWindow(MainWindow.Width, MainWindow.Height);
                MainWindow.Hide();
                gameWin.Show();
                currentWindow = gameWin;

                var grid = new Grid();
                gameWin.Content = grid;

                field = DataWorker.LoadField("field_save.txt");
                Player.CreateNewPlayer();
                DataWorker.LoadPlayer("plyer_save.txt");

                SetGameWindow();
                UpdateGuessedWordsList();
            }
            else
            {
                ShowMessageBox(400,100,"Нет сохранённых игр");
            }
        }

        private static void ShowMessageBox(int width, int height, string text)
        {
            var window = new Window();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Width = width;
            window.Height = height;
            window.Content = new TextBlock()
            {
                Text = text,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                TextWrapping = TextWrapping.Wrap
            };

            window.Show();
        }

        private static void SetDefinitionToGrid(Grid grid, int x, int y)
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
        private static void SetFieldOnCanvas(Canvas canvas)
        {
            cellsList = new List<Cell>();
            for (int y = 0; y < field.YSize; y++)
            {
                for (int x = 0; x < field.XSize; x++)
                {
                    cellsList.Add(new Cell(x, y, field.CellLetter[x, y], field.CellColor[x, y], canvas));
                }
            }
            
        }
        private static void SetCellsColorFromField()
        {
            foreach(Cell cell in cellsList)
            {
                cell.Color = field.CellColor[cell.X, cell.Y];
            }
        }
        private static void UpdateScore()
        {
            ((TextBlock)((StackPanel)((Grid)currentWindow.Content).Children[1]).Children[0]).Text = "Очки: " + Player.Score;
        }
        private static void UpdateGuessedWordsList()
        {
            var spWordsList = (StackPanel)((StackPanel)((Grid)currentWindow.Content).Children[1]).Children[2];
            spWordsList.Children.Clear();
            for (int i = 0; i < Player.WordsList.Count; i++)
            {
                spWordsList.Children.Add(new TextBlock()
                {
                    Text = Player.WordsList[i],
                    FontSize = 15,
                    Width = 20 * 15,
                    Margin = new Thickness(5, 1, 0, 0),
                    Background = Brushes.LightGray,
                    TextAlignment = TextAlignment.Center
                });
            }
        }
        private static void CheckWin()
        {
            if (Player.WordsList.Count == field.WordsList.Count)
            {
                LogicMethods.ActionsIfWin(field);

                ShowMessageBox(400, 50, "Вы отгодали все слова!");
            }
        }
    }
}
