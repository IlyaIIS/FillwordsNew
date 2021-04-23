using System;

namespace Fillwords
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            DataWorker.ReadWordsFromFile(DataWorker.WordsDictionaryPath);
            DataWorker.ReadUserScoreFromFile(DataWorker.UserScoreSavePath);
            DataWorker.ReadSettingsFromFile(DataWorker.SettingsSavePath);

            //Главный цикл
            do
            {
                Printer.DrawMenu();
                KeyInteractions.DoMenuActions();
            } while (true);
        }
    }
}
