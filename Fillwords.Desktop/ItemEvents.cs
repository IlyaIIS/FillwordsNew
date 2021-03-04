using Avalonia.Controls;
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
            Printer.SetErrorWindow("В разработке");
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
            Printer.SetErrorWindow("В разработке");
            //Printer.SetSettingsWindow();
        }

        static public void btnMainExit_Click(object sender, RoutedEventArgs e)
        {
            Printer.MainWindow.Close();
        }


        static public void btnExitToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            Printer.SetMainWindow();
        }
    }
}
