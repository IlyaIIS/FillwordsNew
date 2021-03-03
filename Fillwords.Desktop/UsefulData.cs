using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fillwords.Desktop
{
    static public class UsefulData
    {
        static public Controls StackPanelItemList { get; private set; }
        static public double MainWindowHeight { get; private set; }
        static void SetUsefulData(Controls stackPanelItemList, double mainWindowHeight)
        {
            StackPanelItemList = stackPanelItemList;
            MainWindowHeight = mainWindowHeight;
        }
    }
}
