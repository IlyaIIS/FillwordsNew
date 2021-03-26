using System;
using System.Collections.Generic;
using System.Text;

namespace Fillwords
{
    static public class MouseInfo
    {
        static private bool isPressed = false;
        static public bool IsPressed { get => isPressed; set => isPressed = value; }
    }
}
