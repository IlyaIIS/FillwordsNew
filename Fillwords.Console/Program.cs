using System;

namespace Fillwords
{
    class Program
    {
        static void Main(string[] args)
        {
            DataWorker.ReadWordsFromFile("words.txt");
            DataWorker.ReadUserScoreFromFile("users_score.txt");
            DataWorker.ReadSettingsFromFile("settings.txt");

            //Главный цикл
            do
            {
                Printer.DrawMenu();
                KeyInteractions.DoMenuActions();
            } while (true);
        }
    }
}
