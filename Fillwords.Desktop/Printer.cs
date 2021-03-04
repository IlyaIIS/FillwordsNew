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

        static public void SetMainWindow()
        {
            int buttonsFontSize = 40;
            int buttonsWidth = 320;

            StackPanelItemList.Clear();

            StackPanelItemList.Add(CreateTitleTextBlock("FILLWORDS", buttonsFontSize * 3));

            StackPanelItemList.Add(CreateButton("НОВАЯ ИГРА", ItemEvents.btnStartNewGame_Click, buttonsFontSize, buttonsWidth));
            StackPanelItemList.Add(CreateButton("ПРОДОЛЖИТЬ", ItemEvents.btnContinue_Click, buttonsFontSize, buttonsWidth));
            StackPanelItemList.Add(CreateButton("РЕЙТИНГ",    ItemEvents.btnRecords_Click,  buttonsFontSize, buttonsWidth));
            StackPanelItemList.Add(CreateButton("НАСТРОЙКИ",  ItemEvents.btnSettings_Click, buttonsFontSize, buttonsWidth));
            StackPanelItemList.Add(CreateButton("ВЫХОД",      ItemEvents.btnMainExit_Click, buttonsFontSize, buttonsWidth));
        }

        static private Button CreateButton(string text, EventDel eventClick, int fontSize = 20, int width = 0, 
            Avalonia.Layout.HorizontalAlignment horizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center)
        {
            var button = new Button();

            button.Content = text;
            button.FontSize = fontSize;
            if (width != 0) button.Width = width;
            button.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(eventClick);
            button.HorizontalAlignment = horizontalAlignment;

            button.HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            
            return button;
        }

        static private TextBlock CreateTitleTextBlock(string text, int fontSize)
        {
            var tbTitle = new TextBlock();
            tbTitle.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            tbTitle.FontSize = fontSize;
            tbTitle.FontStyle = Avalonia.Media.FontStyle.Italic;
            tbTitle.Text = "FILLWORDS";
            return tbTitle;
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

        static public void SetRecordsWindow()
        {
            StackPanelItemList.Clear();

            StackPanelItemList.Add(CreateButton("Назад", ItemEvents.btnExitToMainWindow_Click, horizontalAlignment : Avalonia.Layout.HorizontalAlignment.Right));

            StackPanelItemList.Add(CreateTitleTextBlock("РЕКОРДЫ", 50));

            foreach (var user in DataWorker.UserScoreDict)
                StackPanelItemList.Add(CreateTextBlock(user.Key + ": " + user.Value));
        }

        static private TextBlock CreateTextBlock(string text)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;

            return textBlock;
        }

        static public void SetSettingsWindow()
        {
            var settingsPropertyArr = new string[] 
            { 
                "Ширина поля",
                "Высота поля",
                "Размер ячейки",
                "Цвет поля",
                "Цвет текущей ячейки под курсором",
                "Цвет выделенного слова",
                "Цвет отгаданных слов",
                "Случайный цвет отгаданных слов",
                "Установить настройки по умолчанию" 
            };

            StackPanelItemList.Clear();

            StackPanelItemList.Add(CreateButton("Назад", ItemEvents.btnExitToMainWindow_Click, horizontalAlignment: Avalonia.Layout.HorizontalAlignment.Right));

            StackPanelItemList.Add(CreateTitleTextBlock("НАСТРОЙКИ", 50));

            for (int i = 0; i < settingsPropertyArr.Length; i++)
            {
                StackPanelItemList.Add(CreateSettingsDockPanel(settingsPropertyArr[i], Settings.property[i].ToString()));
            }
        }

        static private DockPanel CreateSettingsDockPanel(string text, string score)
        {
            DockPanel dockPanel = new DockPanel();

            var textBlock = new TextBlock();
            textBlock.Text = text;

            var btnLeft = new Button();
            btnLeft.Content = "<";

            var btnRight = new Button();
            btnRight.Content = ">";

            var tbScore = new TextBox();
            tbScore.Text = score;

            dockPanel.Children.Add(textBlock);
            dockPanel.Children.Add(btnLeft);
            dockPanel.Children.Add(tbScore);
            dockPanel.Children.Add(btnRight);

            dockPanel.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right;

            return dockPanel;
        }
    }
}
