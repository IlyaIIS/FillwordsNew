using System.Collections.Generic;

namespace Fillwords
{
    static class Player
    {
        static public int x, y;
        static public int preX, preY;
        static public List<int[]> coordStory = new List<int[]>();
        static public string wordNow;
        static public List<string> wordsList = new List<string>();
        static public int score;
        static public string name;

        static public void CreateNewPlayer()
        {
            x = 0;
            y = 0;
            preX = 0;
            preY = 0;
            coordStory.RemoveRange(0, coordStory.Count);
            wordNow = string.Empty;
            wordsList.RemoveRange(0, wordsList.Count);
            score = 0;
        }
    }
}
