using System.Collections.Generic;

namespace Fillwords
{
    public static class Player
    {
        static public int X { get; set; }
        static public int Y { get; set; }
        static public int PreX { get; set; }
        static public int PreY { get; set; }
        static public List<int[]> CoordStory { get; set; } = new List<int[]>();
        static public string WordNow { get; set; }
        static public List<string> WordsList { get; set; } = new List<string>();
        static public int Score { get; set; }
        static public string Name { get; set; }

        static public void CreateNewPlayer()
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
