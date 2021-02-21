using System;

namespace Fillwords
{
    class KeyInteractions
    {
        static public void DoMenuActions()
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

        static private void SelectMenuItem(int position)
        {
            Console.Clear();

            if (position == 1)
            {
                GetUserName();

                Field field = new Field();
                field.CreateNewField(Settings.xSize, Settings.ySize, new WordsSet(DataWorker.wordsSet.allWords));
                Player.CreateNewPlayer();
                DoGameActions(field);
            }
            if (position == 2)
            {
                if (DataWorker.saveExist("field_save.txt", "plyer_save.txt"))
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

        static private void GetUserName()
        {
            string userName;

            Console.SetCursorPosition(Printer.GetIndent(28).Length, Console.WindowHeight / 2);
            Console.Write("Введите имя: ");
            do
            {
                Console.SetCursorPosition(Printer.GetIndent(2).Length, Console.WindowHeight / 2);
                userName = Console.ReadLine();
            } while (userName.Length == 0);

            Player.name = userName;

            Console.Clear();
        }

        static private void DoGameActions(Field field)
        {
            string[] allWords = DataWorker.wordsSet.allWords;
            
            Printer.DrawField(field);
            Printer.DrawFieldItem(0, 0, Settings.Colors[Settings.underCursorColor, 0],
                                        Settings.Colors[Settings.underCursorColor, 1], field);
            Printer.DrawScore(Player.score);
            for(int i = 0; i < Player.wordsList.Count; i++)
                Printer.DrawText(Player.wordsList[i], i);

            ConsoleKeyInfo key;
            bool isEnter = false;
            bool isCheat = false;

            do
            {
                Player.preX = Player.x;
                Player.preY = Player.y;

                key = Console.ReadKey(true);

                MoveCursorInField(field,  key);

                //Если курсор сдвинулся
                if (Player.preX != Player.x || Player.preY != Player.y)
                    GameLogicMethods.PlayerMoveAction(field, isEnter);

                //Действия при enter или space
                if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
                    GameLogicMethods.PlayerEnterAction(field, ref isEnter, allWords);

                //Действия при esc
                if (key.Key == ConsoleKey.Escape)
                {
                    if (isEnter)
                    {
                        GameLogicMethods.BrakeFilling(field);
                        
                        Printer.DrawText(new string(' ', Console.WindowWidth - (field.xSize * 4 + 2)), Player.wordsList.Count);

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
                            break;
                        }
                        else
                        {
                            Console.Clear();
                            Printer.DrawField(field);
                            Printer.DrawFieldItem(Player.x, Player.y, ConsoleColor.DarkGray, ConsoleColor.White, field);
                            for (int i = 0; i < Player.wordsList.Count; i++)
                                Printer.DrawText(Player.wordsList[i], i);
                        }
                    }
                }

                //Проверка на победу
                if (Player.wordsList.Count == field.wordsList.Count)
                {
                    if (DataWorker.userScoreDict.ContainsKey(Player.name))
                        DataWorker.userScoreDict[Player.name] += Player.score;
                    else
                        DataWorker.userScoreDict.Add(Player.name, Player.score);

                    DataWorker.UpdateUsetScoreFile("users_score.txt");
                    if (field.isLoaded) DataWorker.DeliteSave("field_save.txt", "plyer_save.txt");

                    Printer.DrawPopupWindow("Вы отгодали все слова!");
                    Console.ReadKey(true);
                    break;
                }

                //Если С (чит)
                if (key.Key == ConsoleKey.C)
                {
                    isCheat = true;
                    Random rnd = new Random();
                    int randomNum = rnd.Next(field.wordsList.Count);

                    for (int ii = 0; ii < field.wordsList[randomNum].Length; ii++)
                    {
                        int x = field.wordPos[randomNum][ii].X;
                        int y = field.wordPos[randomNum][ii].Y;
                        Printer.DrawFieldItem(x, y, ConsoleColor.Yellow, ConsoleColor.Red, field);
                    }
                }
                else
                if (isCheat)
                {
                    isCheat = false;
                    for (int i = 0; i < field.wordsList.Count; i++)
                        for (int ii = 0; ii < field.wordsList[i].Length; ii++)
                        {
                            int x = field.wordPos[i][ii].X;
                            int y = field.wordPos[i][ii].Y;
                            Printer.DrawFieldItem(x, y, field.cellColor[x, y, 0], field.cellColor[x, y, 1], field);
                        }

                    Printer.DrawFieldItem(Player.x, Player.y, ConsoleColor.DarkGray, ConsoleColor.White, field);
                }

            } while (true);
        }

        static private void MoveCursorInField(Field field, ConsoleKeyInfo key)
        {
            if (Player.x < field.xSize - 1 && (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)) Player.x++;
            if (Player.y > 0               && (key.Key == ConsoleKey.UpArrow    || key.Key == ConsoleKey.W)) Player.y--;
            if (Player.x > 0               && (key.Key == ConsoleKey.LeftArrow  || key.Key == ConsoleKey.A)) Player.x--;
            if (Player.y < field.ySize - 1 && (key.Key == ConsoleKey.DownArrow  || key.Key == ConsoleKey.S)) Player.y++;
        }

        static private void DoSettingsActions()
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

        static private void MoveCursorInSettingsMeny(ref int position, ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.UpArrow && position >= 1) position--;
            if (key.Key == ConsoleKey.DownArrow && position < 8) position++;
        }

        static private void ChangeSetting(int position, ConsoleKeyInfo key)
        {
            if (position == 0 || position == 1) 
            {
                if (key.Key == ConsoleKey.LeftArrow && (int)Settings.property[position] > 3)
                    Settings.property[position] = (int)Settings.property[position] - 1;
                if (key.Key == ConsoleKey.RightArrow && (int)Settings.property[position] < 15)
                    Settings.property[position] = (int)Settings.property[position] + 1;
            }

            if (position == 2)
            {
                if (key.Key == ConsoleKey.LeftArrow && (int)Settings.property[position] > 1)
                    Settings.property[position] = (int)Settings.property[position] - 1;
                if (key.Key == ConsoleKey.RightArrow && (int)Settings.property[position] < 3)
                    Settings.property[position] = (int)Settings.property[position] + 1;
            }

            if (position >= 3 && position != 7)
            {
                if (key.Key == ConsoleKey.LeftArrow)
                    Settings.property[position] = (int)Settings.property[position] - 1;
                if (key.Key == ConsoleKey.RightArrow)
                    Settings.property[position] = (int)Settings.property[position] + 1;
            }

            if (position == 7)
            {
                if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.RightArrow)
                    Settings.property[position] = !(bool)Settings.property[position];
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
