using System;

namespace Fillwords
{
    public static class LogicMethods
    {
        public static void ActionsIfWordSelected(Field field, int color)
        {
            for (int i = 0; i < Player.CoordStory.Count; i++)
            {
                int x = Player.CoordStory[i][0];
                int y = Player.CoordStory[i][1];

                field.CellColor[x, y] = color;
                field.CellColor[x, y] = color;
            }

            Player.WordsList.Add(Player.WordNow);
            Player.Score += (int)Math.Pow(Player.WordNow.Length, 1.5);
        }

        public static void ActionsIfWin(Field field)
        {
            if (DataWorker.UserScoreDict.ContainsKey(Player.Name))
                DataWorker.UserScoreDict[Player.Name] += Player.Score;
            else
                DataWorker.UserScoreDict.Add(Player.Name, Player.Score);

            DataWorker.UpdateUsetScoreFile(DataWorker.UserScoreSavePath);
            if (field.IsLoaded)
            {
                DataWorker.DeleteSave(DataWorker.FieldSavePath);
                DataWorker.DeleteSave(DataWorker.PlayerSavePath);
            }
        }
    }
}
