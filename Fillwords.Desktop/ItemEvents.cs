using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

namespace Fillwords.Desktop
{
    public static class ItemEvents
    {
        public static void btnStartNewGame_Click(object sender, RoutedEventArgs e)
        {
            Printer.SetNewGameWindow();
        }

        public static void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            Printer.SetContinueWindow();
        }

        public static void btnRecords_Click(object sender, RoutedEventArgs e)
        {
            Printer.SetRecordsWindow();
        }

        public static void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Printer.SetSettingsWindow();
        }

        public static void btnMainExit_Click(object sender, RoutedEventArgs e)
        {
            Printer.MainWindow.Close();
        }

        public static void btnExitToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            Printer.SetMainWindow();
        }

        public static void wAdditionalWindow_Close(object sender, EventArgs e)
        {
            Printer.SetMainWindow();
        }

        public static void tbEnter_Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ((TextBox)sender).Text != null)
            {
                Player.Name = ((TextBox)sender).Text;
                Printer.SetGameWindow();
            }
        }

        public static void wSettingsWindow_Close(object? sender, EventArgs e)
        {
            DataWorker.UpdateSettingsFile("settings.txt");
            Printer.SetMainWindow();
        }
    }
}
