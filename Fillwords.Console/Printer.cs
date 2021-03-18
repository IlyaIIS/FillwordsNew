using System;

namespace Fillwords
{
    class Printer
    {
        static public void DrawMenu()
        {
            Console.Clear();
            Console.SetWindowSize(120, 35);
            DrawTitle();
            DrawMenuItem(1, true);
            DrawMenuItem(2, false);
            DrawMenuItem(3, false);
            DrawMenuItem(4, false);
            DrawMenuItem(5, false);
        }
        static void DrawTitle()
        {
            string indent = GetIndent(68);

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine();
            Console.WriteLine(indent + "██████      ▄▄  ▄▄                                        ▄▄        ");
            Console.WriteLine(indent + "██      ██  ██  ██  ██          ██   ▄████▄   ██ ▄▄▄▄     ██  ▄█████");
            Console.WriteLine(indent + "██████      ██  ██  ██▄   ▄▄   ▄██  ██▀  ▀██  ████▀▀▀     ██  ██    ");
            Console.WriteLine(indent + "██      ██  ██  ██   ██   ██   ██   ██    ██  ██▀     ▄█████  ▀████▄");
            Console.WriteLine(indent + "██      ██  ██  ██   ▀██▄████▄██▀   ██▄  ▄██  ██      ██  ██      ██");
            Console.WriteLine(indent + "██      ██  ██  ██    ▀██▀  ▀██▀     ▀████▀   ██      ▀█████  █████▀");
            Console.WriteLine();
            Console.WriteLine();

            Console.ResetColor();
        }

        static public void DrawMenuItem(int num, bool isHighlighting)
        {
            string indent = GetIndent(47);

            if (!isHighlighting) Console.ForegroundColor = ConsoleColor.DarkGray;

            if (num == 1)
            {
                Console.WriteLine();
                Console.WriteLine(indent + "    █  █ ▄▀▀▄ █▀▄ ▄▀▄ █▀▄  █  █ ███ █▀▄ ▄▀▄");
                Console.WriteLine(indent + "    █▄▄█ █  █ █▀▄ █▄█ █▄▀  █▄▀█ █   █▄▀ █▄█");
                Console.WriteLine(indent + "    █  █ ▀▄▄▀ █▄▀ █ █ █ █  █  █ █   █   █ █");
                Console.WriteLine();
            }

            if (num == 2)
            {
                Console.WriteLine();
                Console.WriteLine(indent + "████ █▀▄ ▄▀▀▄  ▄▀█  ▄▀▀▄ ▄▀▄ █ █ █ █  █ ███ █  ");
                Console.WriteLine(indent + "█  █ █▄▀ █  █  █ █  █  █ █ █  ███  █▄▀█  █  █▀▄");
                Console.WriteLine(indent + "█  █ █   ▀▄▄▀ █▀▀▀█ ▀▄▄▀ █ █ █ █ █ █  █  █  █▄▀");
                Console.WriteLine();
            }

            if (num == 3)
            {
                Console.WriteLine(indent + "                 ▀▀                   ");
                Console.WriteLine(indent + "        █▀▄ █▀▀ █  █ ███ █  █ █  █ ███");
                Console.WriteLine(indent + "        █▄▀ █▄▄ █▄▀█  █  █▄▀█ █▄▄█ █  ");
                Console.WriteLine(indent + "        █   █▄▄ █  █  █  █  █ █  █ █  ");
                Console.WriteLine();
            }

            if (num == 4)
            {
                Console.WriteLine(indent + "                              ▀▀           ");
                Console.WriteLine(indent + "   █  █ ▄▀▄ █▀▀ ███ █▀▄ ▄▀▀▄ █  █ █ ▄█ █  █");
                Console.WriteLine(indent + "   █▄▄█ █▄█ █    █  █▄▀ █  █ █▄▀█ ███  █▄▀█");
                Console.WriteLine(indent + "   █  █ █ █ █▄▄  █  █   ▀▄▄▀ █  █ █ ▀█ █  █");
                Console.WriteLine();
            }

            if (num == 5)
            {
                Console.WriteLine();
                Console.WriteLine(indent + "           █▀█ █   █ █ █ ▄▀▀▄  ▄▀█ ");
                Console.WriteLine(indent + "           █▀▄ █▀▄ █  █  █  █  █ █ ");
                Console.WriteLine(indent + "           █▄█ █▄▀ █ █ █ ▀▄▄▀ █▀▀▀█");
                Console.WriteLine();
            }

            Console.ResetColor();
        }

        static public string GetIndent(int textSize)
        {
            return new string(' ', (Console.WindowWidth - textSize) / 2);
        }

        static public void DrawField(Field field)
        {
            Console.BackgroundColor = ColorsSet.ColorsList[Settings.FieldColor, 0];
            Console.ForegroundColor = ColorsSet.ColorsList[Settings.FieldColor, 1];

            DrawFieldLine("┌", "─", "┬", "┐", field.XSize);
            Console.WriteLine();

            for (int y = 0; y < field.YSize; y++)
            {
                Console.Write('│');
                for (int x = 0; x < field.XSize; x++)
                {
                    Console.Write(" ");
                    Console.BackgroundColor = ColorsSet.ColorsList[field.CellColor[x, y], 0];
                    Console.ForegroundColor = ColorsSet.ColorsList[field.CellColor[x, y], 1];
                    Console.Write(field.CellLetter[x, y]);
                    Console.BackgroundColor = ColorsSet.ColorsList[Settings.FieldColor, 0];
                    Console.ForegroundColor = ColorsSet.ColorsList[Settings.FieldColor, 1];
                    Console.Write(" " + "│");
                }
                Console.WriteLine();
                DrawFieldLine("├", "─", "┼", "┤", field.XSize);
                Console.WriteLine();
            }

            Console.SetCursorPosition(0, Console.CursorTop - 1);
            DrawFieldLine("└", "─", "┴", "┘", field.XSize);

            Console.ResetColor();
        }

        static void DrawFieldLine(string sign1, string sign2, string sign3, string sign4, int num)
        {
            Console.Write(sign1 + sign2 + sign2 + sign2);
            for (int i = 0; i < num - 1; i++) Console.Write(sign3 + sign2 + sign2 + sign2);
            Console.Write(sign4);
        }

        static public void DrawFieldItem(int x, int y, ConsoleColor color1, ConsoleColor color2, Field field)
        {
            Console.SetCursorPosition(x * 4 + 2, y * 2 + 1);
            Console.BackgroundColor = color1;
            Console.ForegroundColor = color2;
            Console.Write(field.CellLetter[x, y]);
            Console.ResetColor();
        }

        static public void DrawText(string text, int num)
        {
            Console.SetCursorPosition(Settings.XSize*4 + 2, num + 1);
            Console.Write(text);
        }

        static public void DrawScore(int score)
        {
            Console.SetCursorPosition(0, Settings.YSize + Settings.YSize * 1 + 3);
            Console.Write("Счёт: " + score);
        }

        static public void DrawPopupWindow(string text)
        {
            Console.SetCursorPosition(Console.WindowWidth/2 - text.Length / 2 - 1, Console.WindowHeight / 2 - 1);
            Console.Write("╔" + new string('═', text.Length) + "╗");
            Console.SetCursorPosition(Console.WindowWidth/2 - text.Length / 2 - 1, Console.WindowHeight / 2);
            Console.Write("║" + text + "║");
            Console.SetCursorPosition(Console.WindowWidth/2 - text.Length / 2 - 1, Console.WindowHeight / 2 + 1);
            Console.Write("╚" + new string('═', text.Length) + "╝");
        }

        static public void DrawTableOfRecords()
        {
            DrawMenuItem(3, true);
            foreach(var user in DataWorker.UserScoreDict)
            {
                Console.WriteLine(user.Key + ": " + user.Value);
            }
        }

        static public void DrawSettings()
        {
            Console.SetCursorPosition(0, 0);
            DrawSettringsItem(0, true);
            DrawSettringsItem(1, false);
            DrawSettringsItem(2, false);
            DrawSettringsItem(3, false);
            DrawSettringsItem(4, false);
            DrawSettringsItem(5, false);
            DrawSettringsItem(6, false);
            DrawSettringsItem(7, false);
            DrawSettringsItem(8, false);
        }

        static public void DrawSettringsItem(int property, bool isHighlighting)
        {
            if (!isHighlighting) Console.ForegroundColor = ConsoleColor.DarkGray;

            string text;

            switch (property)
            {
                case 0: text = "Ширина поля";
                    break;
                case 1: text = "Высота поля";
                    break;
                case 2: text = "Размер ячейки (в разработке)";
                    break;
                case 3: text = "Цвет поля";
                    break;
                case 4: text = "Цвет текущей ячейки под курсором";
                    break;
                case 5: text = "Цвет выделенного слова";
                    break;
                case 6: text = "Цвет отгаданных слов";
                    break;
                case 7: text = "Случайный цвет отгаданных слов";
                    break;
                case 8: text = "Установить настройки по умолчанию";
                    break;
                default: text = String.Empty;
                    break;
            }

            if (property >= 0 && property <= 2)
            {
                Console.Write(text + $" < {Settings.Property[property]} >   ");
            }
            else if (property <= 6)
            {
                Console.Write(text);
                Console.Write(" <     > ");

                Console.SetCursorPosition(Console.CursorLeft - 6, Console.CursorTop);
                Console.BackgroundColor = ColorsSet.ColorsList[(int)Settings.Property[property], 0];
                Console.ForegroundColor = ColorsSet.ColorsList[(int)Settings.Property[property], 1];
                Console.Write(" A ");
                Console.ResetColor();
            }
            else if (property == 7)
            {
                Console.Write(text + $" < {((bool)Settings.Property[7] ? "Да" : "Нет")} >   ");
            }
            else if (property == 8)
            {
                Console.Write(text);
            }

            Console.WriteLine();
            Console.ResetColor();
        }
    }

    public class ColorsSet
    {
        static public dynamic[,] ColorsList =
        {
            { ConsoleColor.Black    , ConsoleColor.White },
            { ConsoleColor.DarkGray , ConsoleColor.White },
            { ConsoleColor.Gray     , ConsoleColor.Black },
            { ConsoleColor.DarkBlue , ConsoleColor.White },
            { ConsoleColor.DarkGreen, ConsoleColor.White },
            { ConsoleColor.DarkCyan , ConsoleColor.White },
            { ConsoleColor.DarkRed  , ConsoleColor.White },
            { ConsoleColor.DarkMagenta, ConsoleColor.White },
            { ConsoleColor.DarkYellow , ConsoleColor.Black },
            { ConsoleColor.Blue     , ConsoleColor.Black },
            { ConsoleColor.Green    , ConsoleColor.Black },
            { ConsoleColor.Cyan     , ConsoleColor.Black },
            { ConsoleColor.Red      , ConsoleColor.White },
            { ConsoleColor.Magenta  , ConsoleColor.White },
            { ConsoleColor.Yellow   , ConsoleColor.Black }
        };

        public ConsoleColor this[int index1, int index2]
        {
            get
            {
                return ColorsList[Math.Abs(index1) % (ColorsList.Length / ColorsList.Rank), Math.Abs(index2) % 2];
            }
        }

        public static int GetRandomColor()
        {
            Random rnd = new Random();
            int randomNum;

            do
            {
                randomNum = rnd.Next(ColorsList.Length / ColorsList.Rank);
            } while (randomNum == Settings.FieldColor || randomNum == Settings.UnderCursorColor ||
                                                         randomNum == Settings.PickedWordColor);

            return randomNum;
        }
    }
}
