using System.Collections.Generic;

namespace Fillwords
{
    public static class Player
    {
        public static int X { get; set; }
        public static int Y { get; set; }
        public static int PreX { get; set; }
        public static int PreY { get; set; }
        public static List<int[]> CoordStory { get; set; } = new List<int[]>();
        public static string WordNow { get; set; }
        public static List<string> WordsList { get; set; } = new List<string>();
        public static int Score { get; set; }
        public static string Name { get; set; }

        public static void CreateNewPlayer()
        {
            X = 0;
            Y = 0;
            PreX = 0;
            PreY = 0;
            CoordStory.Clear();
            WordNow = string.Empty;
            WordsList.Clear();
            Score = 0;
        }
    }
}
