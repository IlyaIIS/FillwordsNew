using Avalonia.Media;
using System;

namespace Fillwords.Desktop
{
    public class ColorsSet
    {
        public static ISolidColorBrush[,] ColorsList =
        {
            { Brushes.Black    , Brushes.White },
            { Brushes.DarkGray , Brushes.White },
            { Brushes.Gray     , Brushes.Black },
            { Brushes.DarkBlue , Brushes.White },
            { Brushes.DarkGreen, Brushes.White },
            { Brushes.DarkCyan , Brushes.White },
            { Brushes.DarkRed  , Brushes.White },
            { Brushes.DarkMagenta, Brushes.White },
            { Brushes.Orange   , Brushes.Black },
            { Brushes.Blue     , Brushes.Black },
            { Brushes.Green    , Brushes.Black },
            { Brushes.Cyan     , Brushes.Black },
            { Brushes.Red      , Brushes.White },
            { Brushes.Magenta  , Brushes.White },
            { Brushes.Yellow   , Brushes.Black }
        };

        public ISolidColorBrush this[int index1, int index2]
        {
            get
            {
                return ColorsList[Math.Abs(index1) % (ColorsList.Length / ColorsList.Rank), Math.Abs(index2) % 2];
            }
        }

        public static int GetRandomColor()
        {
            Random rnd = new Random();
            int randomNum;

            do
            {
                randomNum = rnd.Next(ColorsList.Length / ColorsList.Rank);
            } while (randomNum == Settings.FieldColor || 
                     randomNum == Settings.UnderCursorColor ||
                     randomNum == Settings.PickedWordColor);

            return randomNum;
        }
    }
}
