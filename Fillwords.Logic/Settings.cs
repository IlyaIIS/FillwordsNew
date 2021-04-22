using System;

namespace Fillwords
{
    public static class Settings
    {
        public static int XSize { get; set; }
        public static int YSize { get; set; }
        public static int CellSize { get; set; }
        public static int FieldColor { get; set; }
        public static int UnderCursorColor { get; set; }
        public static int PickedWordColor { get; set; }
        public static int GuessedWordColor { get; set; }
        public static bool IsRandomGuessedWordColor { get; set; }

        public static SettingsIndexer Property { get; private set; } = new SettingsIndexer();
        public static readonly int[] MinValueOfProperty = new int[] { 3, 3, 5, 0, 0, 0, 0 };
        public static readonly int[] MaxValueOfProperty = new int[] { 15, 15, 60, 14, 14, 14, 14 };
        public static int WordNumMin { get; private set; } = 2;

        public static void SetDefaultSettings()
        {
            XSize = 10;
            YSize = 10;
            CellSize = 30;
            FieldColor = 0;
            UnderCursorColor = 1;
            PickedWordColor = 2;
            GuessedWordColor = 4;
            IsRandomGuessedWordColor = true;
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
                    case 0: return Settings.XSize;
                    case 1: return Settings.YSize;
                    case 2: return Settings.CellSize;
                    case 3: return Settings.FieldColor;
                    case 4: return Settings.UnderCursorColor;
                    case 5: return Settings.PickedWordColor;
                    case 6: return Settings.GuessedWordColor;
                    case 7: return Settings.IsRandomGuessedWordColor;
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
                            case 7: Settings.IsRandomGuessedWordColor = (bool)value; break;
                        }
                    }
                }
                else if (index == 7)
                    Settings.IsRandomGuessedWordColor = (bool)value;
            }
        }
    }
}
