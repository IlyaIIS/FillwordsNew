using System;

namespace Fillwords
{
    public static class Settings
    {
        static public int xSize = 10;
        static public int ySize = 10;
        static public int cellSize = 1;
        static public int fieldColor = 0;
        static public int underCursorColor = 1;
        static public int pickedWordColor = 2;
        static public int guessedWordColor = 4;
        static public bool isRandomGuessedWordColro = true;

        static public SettingsIndexer property = new SettingsIndexer();
        static public int wordNumMin = 2;

        static public void SetDefaultSettings()
        {
            xSize = 10;
            ySize = 10;
            cellSize = 1;
            fieldColor = 0;
            underCursorColor = 1;
            pickedWordColor = 2;
            guessedWordColor = 4;
            isRandomGuessedWordColro = true;
        }
    }

    public class SettingsIndexer
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
                    case 5: return Settings.pickedWordColor;
                    case 6: return Settings.guessedWordColor;
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
                    case 5: Settings.pickedWordColor          = (int)value; break;
                    case 6: Settings.guessedWordColor         = (int)value; break;
                    case 7: Settings.isRandomGuessedWordColro = (bool)value; break;
                }
            }
        }
    }
}
