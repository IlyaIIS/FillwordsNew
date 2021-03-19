using System;
using System.Collections.Generic;

namespace Fillwords
{
    public static class Settings
    {
        static public int XSize { get; set; }
        static public int YSize { get; set; }
        static public int CellSize { get; set; }
        static public int FieldColor { get; set; }
        static public int UnderCursorColor { get; set; }
        static public int PickedWordColor { get; set; }
        static public int GuessedWordColor { get; set; }
        static public bool IsRandomGuessedWordColro { get; set; }

        static public SettingsIndexer Property = new SettingsIndexer();
        static public int WordNumMin = 2;

        static public void SetDefaultSettings()
        {
            XSize = 10;
            YSize = 10;
            CellSize = 30;
            FieldColor = 0;
            UnderCursorColor = 1;
            PickedWordColor = 2;
            GuessedWordColor = 4;
            IsRandomGuessedWordColro = true;
        }

        static public readonly List<int> MinValueOfProperty = new List<int>() { 3, 3, 5, 0, 0, 0, 0 };
        static public readonly List<int> MaxValueOfProperty = new List<int>() { 15, 15, 60, 14, 14, 14, 14 };
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
                    case 0: return Settings.XSize;
                    case 1: return Settings.YSize;
                    case 2: return Settings.CellSize;
                    case 3: return Settings.FieldColor;
                    case 4: return Settings.UnderCursorColor;
                    case 5: return Settings.PickedWordColor;
                    case 6: return Settings.GuessedWordColor;
                    case 7: return Settings.IsRandomGuessedWordColro;
                    default: return 0;
                }
            }
            set
            {
                if (index >= 0 && index <= 6)
                {
                    if ((int)value >= Settings.MinValueOfProperty[index] && (int)value <= Settings.MaxValueOfProperty[index])
                    {
                        switch (index)
                        {
                            case 0: Settings.XSize = (int)value; break;
                            case 1: Settings.YSize = (int)value; break;
                            case 2: Settings.CellSize = (int)value; break;
                            case 3: Settings.FieldColor = (int)value; break;
                            case 4: Settings.UnderCursorColor = (int)value; break;
                            case 5: Settings.PickedWordColor = (int)value; break;
                            case 6: Settings.GuessedWordColor = (int)value; break;
                            case 7: Settings.IsRandomGuessedWordColro = (bool)value; break;
                        }
                    }
                }
                else if (index == 7)
                    Settings.IsRandomGuessedWordColro = (bool)value;
            }
        }
    }
}
