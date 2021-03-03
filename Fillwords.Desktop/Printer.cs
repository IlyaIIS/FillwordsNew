using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fillwords.Desktop
{
    static public class Printer
    {
        static public Controls StackPanelItemList { get; private set; }
        static public Window MainWindow { get; private set; }
        delegate void EventDel(object sender, RoutedEventArgs e);
        static public void SetPrinterParams(Controls stackPanelItemList, Window mainWindow)
        {
            StackPanelItemList = stackPanelItemList;
            MainWindow = mainWindow;
        }

        static public void SetMainWindow(Controls stackPanelItemList)
        {
            int buttonsFontSize = 40;
            int buttonsWidth = 320;

            stackPanelItemList.Clear();

            var tbTitle = new TextBlock();
            tbTitle.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            tbTitle.FontSize = buttonsFontSize * 3;
            tbTitle.FontStyle = Avalonia.Media.FontStyle.Italic;
            tbTitle.Text = "FILLWORDS";
            stackPanelItemList.Add(tbTitle);

            stackPanelItemList.Add(CreateButton("НОВАЯ ИГРА", buttonsFontSize, buttonsWidth, ItemEvents.btnStartNewGame_Click));
            stackPanelItemList.Add(CreateButton("ПРОДОЛЖИТЬ", buttonsFontSize, buttonsWidth, ItemEvents.btnContinue_Click));
            stackPanelItemList.Add(CreateButton("РЕЙТИНГ",    buttonsFontSize, buttonsWidth, ItemEvents.btnRecords_Click));
            stackPanelItemList.Add(CreateButton("НАСТРОЙКИ",  buttonsFontSize, buttonsWidth, ItemEvents.btnSettings_Click));
            stackPanelItemList.Add(CreateButton("ВЫХОД",      buttonsFontSize, buttonsWidth, ItemEvents.btnExit_Click));
        }

        static private Button CreateButton(string text, int fontSize, int width, EventDel eventClick)
        {
            var button = new Button();

            button.Content = text;
            button.FontSize = fontSize;
            button.Width = width;
            button.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(eventClick);

            button.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            button.HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            
            return button;
        }

        static public void SetErrorWindow(string text)
        {
            StackPanelItemList.Clear();

            var tbErrorText = new TextBlock();
            tbErrorText.Text = text;
            tbErrorText.FontSize = 20;
            tbErrorText.Margin = new Thickness((int)(MainWindow.Height / 4));
            tbErrorText.TextWrapping = TextWrapping.Wrap;
            tbErrorText.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            tbErrorText.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;

            StackPanelItemList.Add(tbErrorText);
        }
    }
}
