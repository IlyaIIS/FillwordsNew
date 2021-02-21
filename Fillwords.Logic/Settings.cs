using System;

namespace Fillwords
{
    static class Settings
    {
        static public int xSize = 10;
        static public int ySize = 10;
        static public int cellSize = 1;
        static public int fieldColor = 0;
        static public int underCursorColor = 1;
        static public int pickedWordColro = 2;
        static public int guessedWordColro = 4;
        static public bool isRandomGuessedWordColro = true;

        static public SettingsIndexer property = new SettingsIndexer();
        static public ColorsSet Colors = new ColorsSet();
        static public int wordNumMin = 2;

        static public void SetDefaultSettings()
        {
            xSize = 10;
            ySize = 10;
            cellSize = 1;
            fieldColor = 0;
            underCursorColor = 1;
            pickedWordColro = 2;
            guessedWordColro = 4;
            isRandomGuessedWordColro = true;
        }
    }

    class SettingsIndexer
    {
        public int lenght = 8;
        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Settings.xSize;
                    case 1: return Settings.ySize;
                    case 2: return Settings.cellSize;
                    case 3: return Settings.fieldColor;
                    case 4: return Settings.underCursorColor;
                    case 5: return Settings.pickedWordColro;
                    case 6: return Settings.guessedWordColro;
                    case 7: return Settings.isRandomGuessedWordColro;
                    default: return 0;
                }
            }
            set
            {
                switch (index)
                {
                    case 0: Settings.xSize                    = (int)value; break;
                    case 1: Settings.ySize                    = (int)value; break;
                    case 2: Settings.cellSize                 = (int)value; break;
                    case 3: Settings.fieldColor               = (int)value; break;
                    case 4: Settings.underCursorColor         = (int)value; break;
                    case 5: Settings.pickedWordColro          = (int)value; break;
                    case 6: Settings.guessedWordColro         = (int)value; break;
                    case 7: Settings.isRandomGuessedWordColro = (bool)value; break;
                }
            }
        }
    }

    class ColorsSet
    {
        static dynamic[,] colorsList =
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

        public ConsoleColor this[int index1,int index2]
        {
            get
            {
                return colorsList[Math.Abs(index1) % (colorsList.Length / colorsList.Rank), Math.Abs(index2) % 2];
            }
        }

        public int GetRandomColor()
        {
            Random rnd = new Random();
            int randomNum;

            do
            {
                randomNum = rnd.Next(colorsList.Length);
            } while (randomNum == Settings.fieldColor || randomNum == Settings.underCursorColor || 
                                                         randomNum == Settings.pickedWordColro);

            return randomNum;
        }
    }
}
