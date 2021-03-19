using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fillwords.Desktop
{
    static public class ItemEvents
    {
        static public void btnStartNewGame_Click(object sender, RoutedEventArgs e)
        {
            Printer.SetNewGameWindowStart();
        }

        static public void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            Printer.SetErrorWindow("В разработке");
        }

        static public void btnRecords_Click(object sender, RoutedEventArgs e)
        {
            Printer.SetRecordsWindow();
        }

        static public void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Printer.SetSettingsWindow();
        }

        static public void btnMainExit_Click(object sender, RoutedEventArgs e)
        {
            Printer.MainWindow.Close();
        }

        static public void btnExitToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            Printer.SetMainWindow();
        }

        static public void wCloseAdditionalWindow(object sender, EventArgs e)
        {
            Printer.SetMainWindow();
        }

        static public void tbEnterEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ((TextBox)sender).Text != null)
            {
                Player.Name = ((TextBox)sender).Text;
                Printer.SetNewGameWindowNext();
            }
        }

        internal static void wCloseSettingsWindow(object? sender, EventArgs e)
        {
            DataWorker.UpdateSettingsFile("settings.txt");
            Printer.SetMainWindow();
        }
    }
}
