using System;

namespace Fillwords
{
    class KeyInteractions
    {
        public static void DoMenuActions()
        {
            ConsoleKeyInfo key;
            int position = 1;
            int prePosition = 1;

            do
            {
                key = Console.ReadKey(true);

                if (position > 1 && (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)) position--;
                if (position < 5 && (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)) position++;

                if (position != prePosition)
                {
                    Console.SetCursorPosition(0, 4 + prePosition * 5);
                    Printer.DrawMenuItem(prePosition, false);

                    Console.SetCursorPosition(0, 4 + position * 5);
                    Printer.DrawMenuItem(position, true);

                    Console.SetCursorPosition(0, 28);

                    prePosition = position;
                }
            } while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Spacebar);

            SelectMenuItem(position);
        }

        private static void SelectMenuItem(int position)
        {
            Console.Clear();

            if (position == 1)
            {
                GetUserName();

                Field field = new Field();
                field.CreateNewField(Settings.XSize, Settings.YSize, new WordsSet(DataWorker.WordsSet.AllWords));
                Player.CreateNewPlayer();
                DoGameActions(field);
            }
            if (position == 2)
            {
                if (DataWorker.IsSaveExist("field_save.txt", "plyer_save.txt"))
                {
                    Field field = DataWorker.LoadField("field_save.txt");
                    Player.CreateNewPlayer();
                    DataWorker.LoadPlayer("plyer_save.txt");
                    DoGameActions(field);
                }
                else
                {
                    Printer.DrawPopupWindow("Нет сохранённых игр");
                    Console.ReadKey(true);
                }
            }
            if (position == 3)
            {
                Printer.DrawTableOfRecords();
                Console.ReadKey(true);
            }
            if (position == 4)
            {
                DoSettingsActions();
            }    
            if (position == 5) Environment.Exit(0);
        }

        private static void GetUserName()
        {
            string userName;

            Console.SetCursorPosition(Printer.GetIndent(28).Length, Console.WindowHeight / 2);
            Console.Write("Введите имя: ");
            do
            {
                Console.SetCursorPosition(Printer.GetIndent(2).Length, Console.WindowHeight / 2);
                userName = Console.ReadLine();
            } while (userName.Length == 0);

            Player.Name = userName;

            Console.Clear();
        }

        private static void DoGameActions(Field field)
        {
            string[] allWords = DataWorker.WordsSet.AllWords;
            
            Printer.DrawField(field);
            Printer.DrawFieldItem(0, 0, ColorsSet.ColorsList[Settings.UnderCursorColor, 0],
                                        ColorsSet.ColorsList[Settings.UnderCursorColor, 1], field);
            Printer.DrawScore(Player.Score);
            for(int i = 0; i < Player.WordsList.Count; i++)
                Printer.DrawText(Player.WordsList[i], i);

            ConsoleKeyInfo key;
            bool isEnter = false;
            bool isCheatActive = false;

            do
            {
                Player.PreX = Player.X;
                Player.PreY = Player.Y;

                key = Console.ReadKey(true);

                MoveCursorInField(field,  key);

                if (Player.PreX != Player.X || Player.PreY != Player.Y)
                    PrinterLogicMetods.PlayerMoveAction(field, isEnter);

                if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
                    PrinterLogicMetods.PlayerEnterAction(field, ref isEnter, allWords);

                bool isBreak;
                IfEscapePressed(field, key, ref isEnter, out isBreak);
                if (isBreak) break;

                CheckWin(field, out isBreak);
                if (isBreak) break;

                //Чит
                if (key.Key == ConsoleKey.C)
                {
                    isCheatActive = true;
                    ActivateCheat(field);
                }
                else if (isCheatActive)
                {
                    isCheatActive = false;
                    DeactivateCheat(field);
                }
            } while (true);
        }
        private static void IfEscapePressed(Field field,ConsoleKeyInfo key, ref bool isEnter, out bool isBreak)
        {
            isBreak = false;

            if (key.Key == ConsoleKey.Escape)
            {
                if (isEnter)
                {
                    PrinterLogicMetods.BrakeFilling(field);
                    Printer.DrawText(new string(' ', Console.WindowWidth - (field.XSize * 4 + 2)), Player.WordsList.Count);

                    isEnter = false;
                }
                else
                {
                    Printer.DrawPopupWindow("Вы уверены, что хотите выйти? (прогресс будет сохранён)");

                    key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
                    {
                        DataWorker.SaveField(field, "field_save.txt");
                        DataWorker.SavePlayer("plyer_save.txt");
                        isBreak = true;
                    }
                    else
                    {
                        Console.Clear();
                        Printer.DrawField(field);
                        Printer.DrawFieldItem(Player.X, Player.Y, ConsoleColor.DarkGray, ConsoleColor.White, field);
                        for (int i = 0; i < Player.WordsList.Count; i++)
                            Printer.DrawText(Player.WordsList[i], i);
                    }
                }
            }
        }
        private static void CheckWin(Field field, out bool isBreak)
        {
            isBreak = false;

            if (Player.WordsList.Count == field.WordsList.Count)
            {
                LogicMethods.ActionsIfWin(field);

                Printer.DrawPopupWindow("Вы отгодали все слова!");
                Console.ReadKey(true);

                isBreak = true;
            }
        }

        private static void ActivateCheat(Field field)
        {
            Random rnd = new Random();
            int randomNum = rnd.Next(field.WordsList.Count);

            for (int ii = 0; ii < field.WordsList[randomNum].Length; ii++)
            {
                int x = field.WordPos[randomNum][ii].X;
                int y = field.WordPos[randomNum][ii].Y;
                Printer.DrawFieldItem(x, y, ConsoleColor.Yellow, ConsoleColor.Red, field);
            }
        }
        private static void DeactivateCheat(Field field)
        {
            for (int i = 0; i < field.WordsList.Count; i++)
            {
                for (int ii = 0; ii < field.WordsList[i].Length; ii++)
                {
                    int x = field.WordPos[i][ii].X;
                    int y = field.WordPos[i][ii].Y;
                    Printer.DrawFieldItem(x, y, ColorsSet.ColorsList[field.CellColor[x, y], 0], ColorsSet.ColorsList[field.CellColor[x, y], 1], field);
                }
            }

            Printer.DrawFieldItem(Player.X, Player.Y, ConsoleColor.DarkGray, ConsoleColor.White, field);
        }

        private static void MoveCursorInField(Field field, ConsoleKeyInfo key)
        {
            if (Player.X < field.XSize - 1 && (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)) Player.X++;
            if (Player.Y > 0               && (key.Key == ConsoleKey.UpArrow    || key.Key == ConsoleKey.W)) Player.Y--;
            if (Player.X > 0               && (key.Key == ConsoleKey.LeftArrow  || key.Key == ConsoleKey.A)) Player.X--;
            if (Player.Y < field.YSize - 1 && (key.Key == ConsoleKey.DownArrow  || key.Key == ConsoleKey.S)) Player.Y++;
        }

        private static void DoSettingsActions()
        {
            Printer.DrawSettings();

            ConsoleKeyInfo key;
            int position = 0;
            int prePosition;

            do
            {
                prePosition = position;

                key = Console.ReadKey(true);

                MoveCursorInSettingsMeny(ref position, key);

                if (prePosition != position)
                {
                    Console.SetCursorPosition(0, prePosition);
                    Printer.DrawSettringsItem(prePosition, false);

                    Console.SetCursorPosition(0, position);
                    Printer.DrawSettringsItem(position, true);
                }

                ChangeSetting(position, key);

            } while (key.Key != ConsoleKey.Escape);

            DataWorker.UpdateSettingsFile("settings.txt");
        }

        private static void MoveCursorInSettingsMeny(ref int position, ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.UpArrow && position >= 1) position--;
            if (key.Key == ConsoleKey.DownArrow && position < 8) position++;
        }

        private static void ChangeSetting(int position, ConsoleKeyInfo key)
        {
            if (position == 0 || position == 1) 
            {
                if (key.Key == ConsoleKey.LeftArrow && (int)Settings.Property[position] > 3)
                    Settings.Property[position] = (int)Settings.Property[position] - 1;
                if (key.Key == ConsoleKey.RightArrow && (int)Settings.Property[position] < 15)
                    Settings.Property[position] = (int)Settings.Property[position] + 1;
            }

            if (position == 2)
            {
                if (key.Key == ConsoleKey.LeftArrow && (int)Settings.Property[position] > 1)
                    Settings.Property[position] = (int)Settings.Property[position] - 1;
                if (key.Key == ConsoleKey.RightArrow && (int)Settings.Property[position] < 3)
                    Settings.Property[position] = (int)Settings.Property[position] + 1;
            }

            if (position >= 3 && position != 7)
            {
                if (key.Key == ConsoleKey.LeftArrow)
                    Settings.Property[position] = (int)Settings.Property[position] - 1;
                if (key.Key == ConsoleKey.RightArrow)
                    Settings.Property[position] = (int)Settings.Property[position] + 1;
            }

            if (position == 7)
            {
                if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.RightArrow)
                    Settings.Property[position] = !(bool)Settings.Property[position];
            }

            if (position == 8)
            {
                if (key.Key == ConsoleKey.Enter)
                {
                    Settings.SetDefaultSettings();
                    Printer.DrawSettings();
                    Console.SetCursorPosition(0, 0);
                    Printer.DrawSettringsItem(0, false);
                }
            }

            Console.SetCursorPosition(0, position);
            Printer.DrawSettringsItem(position, true);
        }
    }
}
