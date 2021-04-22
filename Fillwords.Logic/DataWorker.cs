using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fillwords
{
    public static class DataWorker
    {
        static public WordsSet WordsSet { get; private set; }
        static public Dictionary<string, int> UserScoreDict = new Dictionary<string, int>();

        static public void ReadWordsFromFile(string path)
        {
            if (File.Exists(path))
            {
                var text = File.ReadLines(path);
                if (text.Count() > 0)
                {

                    string[] output = new string[text.Count()];

                    int i = 0;
                    foreach (var word in text)
                    {
                        output[i] = word;
                        i++;
                    }

                    WordsSet = new WordsSet(output);
                }
                else
                {
                    WordsSet = new WordsSet(new string[0]);
                }
            }
            else
            {
                File.Create("words.txt");
                WordsSet = new WordsSet(new string[0]);
            }
        }

        public static void ReadUserScoreFromFile(string path)
        {
            if (File.Exists(path))
            {
                var text = File.ReadLines(path);

                foreach (var word in text)
                    UserScoreDict.Add(word.Split(' ')[0], Convert.ToInt32(word.Split(' ')[1]));
            }
            else
            {
                File.Create(path);
            }
        }

        public static void UpdateUsetScoreFile(string path)
        {
            string output = string.Empty;
            foreach (var user in UserScoreDict)
            {
                output += user.Key + " " + user.Value + "\n";
            }

            output.Remove(output.Length - 1);

            File.WriteAllText(path, output);
        }

        public static void ReadSettingsFromFile(string path)
        {
            if (File.Exists(path))
            {
                var text = File.ReadLines(path);

                int i = 0;
                foreach (var word in text)
                {
                    if (i != 7) Settings.Property[i] = Int32.Parse(word);
                    else Settings.Property[i] = bool.Parse(word);
                    i++;
                }
            }
            else
            {
                Settings.SetDefaultSettings();
                UpdateSettingsFile(path);
            }    
        }

        public static void UpdateSettingsFile(string path)
        {
            string output = string.Empty;
            for (int i = 0; i <= Settings.Property.lenght; i++)
            {
                output += Settings.Property[i] + "\n";
            }

            output.Remove(output.Length - 1);

            File.WriteAllText(path, output);
        }

        public static void SaveField(Field field, string path)
        {
            string data = string.Empty;

            data += " ";
            foreach (string word in field.WordsList)
                data += word + " ";
            data = data.Remove(data.Length - 1);
            data += "\n";

            data += "|/";
            foreach (List<MyVector2> wordCoord in field.WordPos)
            {
                foreach (MyVector2 coord in wordCoord)
                {
                    data += coord.X + " " + coord.Y + "_/";
                }
                data = data.Remove(data.Length - 1);
                data += "|/";
            }
            data = data.Remove(data.Length - 2);
            data += "\n";

            data += field.XSize + " " + field.YSize;
            data += "\n";

            for(int y = 0; y < field.YSize; y++)
                for (int x = 0; x < field.XSize; x++)
                    data += field.CellLetter[x,y];
            data += "\n";

            for (int y = 0; y < field.YSize; y++)
                for (int x = 0; x < field.XSize; x++)
                    data += field.CellColor[x, y] + " ";

            File.WriteAllText(path, data);
        }

        public static Field LoadField(string path)
        {
            Field field = new Field();
            field.IsLoaded = true;
            string[] data = File.ReadAllLines(path);

            //список слов
            foreach (char letter in data[0])
            {
                if (letter != ' ')
                {
                    field.WordsList[field.WordsList.Count - 1] += letter;
                }
                else
                {
                    field.WordsList.Add(string.Empty);
                }
            }

            //координаты слов
            string localStr = string.Empty;
            foreach (char letter in data[1])
            {
                if (letter == '|')
                {
                    field.WordPos.Add(new List<MyVector2>());
                }
                else if (letter == '/')
                {
                    field.WordPos[field.WordPos.Count - 1].Add(new MyVector2());
                }
                else if (letter == '_')
                {
                    MyVector2 coord = field.WordPos[field.WordPos.Count - 1][field.WordPos[field.WordPos.Count - 1].Count - 1];
                    coord.Y = Convert.ToInt32(localStr);
                    field.WordPos[field.WordPos.Count - 1][field.WordPos[field.WordPos.Count - 1].Count - 1] = coord;
                    localStr = string.Empty;
                }
                else if (letter == ' ')
                {
                    MyVector2 coord = field.WordPos[field.WordPos.Count - 1][field.WordPos[field.WordPos.Count - 1].Count - 1];
                    coord.Y = -10;
                    coord.X = Convert.ToInt32(localStr);
                    field.WordPos[field.WordPos.Count - 1][field.WordPos[field.WordPos.Count - 1].Count - 1] = coord;
                    localStr = string.Empty;
                }
                else
                {
                    localStr += letter;
                }
            }

            //размер поля
            string[] local = data[2].Split(' ');
            field.XSize = Convert.ToInt32(local[0]);
            field.YSize = Convert.ToInt32(local[1]);

            //буквы поля
            field.CellLetter = new char[field.XSize, field.YSize];
            for (int i = 0; i < data[3].Length; i++)
            {
                field.CellLetter[i % field.XSize, i / field.XSize] = data[3][i];
            }

            //Цвета поля
            field.CellColor = new int[field.XSize, field.YSize];
            localStr = string.Empty;
            int score = 0;
            for (int i = 0; i < data[4].Length; i++)
            {
                if (data[4][i] == ' ')
                {
                    int x = score % field.XSize;
                    int y = score / field.XSize;
                    field.CellColor[x, y] = Int32.Parse(localStr);
                    localStr = string.Empty;
                    score++;
                }
                else
                {
                    localStr += data[4][i];
                }
            }

            return field;
        }

        public static void SavePlayer(string path)
        {
            string data = string.Empty;

            data += Player.Name + "\n";

            data += Player.Score + "\n";

            foreach(string word in Player.WordsList)
            {
                data += word + " ";
            }

            File.WriteAllText(path, data);
        }

        public static void LoadPlayer(string path)
        {
            string[] data = File.ReadAllLines(path);

            Player.Name = data[0];

            Player.Score = Convert.ToInt32(data[1]);

            if (data.Length > 2)
            {
                string localStr = string.Empty;
                foreach (char letter in data[2])
                {
                    if (letter == ' ')
                    {
                        Player.WordsList.Add(localStr);
                        localStr = string.Empty;
                    }
                    else
                    {
                        localStr += letter;
                    }
                }
            }
        }

        public static void DeliteSave(string path1, string path2)
        {
            File.Delete(path1);
            File.Delete(path2);
        }

        public static bool saveExist(string path1, string path2)
        {
            return File.Exists(path1) && File.Exists(path2);
        }

        private static ConsoleColor GetColorFromName(string text)
        {
            switch (text)
            {
                case "White":
                    return ConsoleColor.White;
                case "Black":
                    return ConsoleColor.Black;
                case "DarkGray":
                    return ConsoleColor.DarkGray;
                case "Gray":
                    return ConsoleColor.Gray;
                case "DarkBlue":
                    return ConsoleColor.DarkBlue;
                case "DarkGreen":
                    return ConsoleColor.DarkGreen;
                case "DarkCyan":
                    return ConsoleColor.DarkCyan;
                case "DarkRed":
                    return ConsoleColor.DarkRed;
                case "DarkMagenta":
                    return ConsoleColor.DarkMagenta;
                case "DarkYellow":
                    return ConsoleColor.DarkYellow;
                case "Blue":
                    return ConsoleColor.Blue; 
                case "Green":
                    return ConsoleColor.Green;
                case "Cyan":
                    return ConsoleColor.Cyan;
                case "Red":
                    return ConsoleColor.Red;
                case "Magenta":
                    return ConsoleColor.Magenta;
                case "Yellow":
                    return ConsoleColor.Yellow;
                default:
                    throw new Exception("Несуществующий цвет");
            }
        } 
    }

    public struct WordsSet
    {
        public string[] AllWords { get; private set; }             //Массив из всех слов
        public List<List<string>> WordsSetList { get; private set; }  //Массив массивов слов, сгрупперованных по длине
        public WordsSet(string[] input)
        {
            this.AllWords = input;
            this.WordsSetList = new List<List<string>>();
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i].Length > this.WordsSetList.Count - 1)
                    {
                        do
                        {
                            this.WordsSetList.Add(new List<string>());
                        } while (this.WordsSetList.Count - 1 < input[i].Length);
                    }

                    this.WordsSetList[input[i].Length].Add(input[i]);
                }
            }
        }
    }

    public struct MyVector2
    {
        public int X { get; set; }
        public int Y { get; set; }

        public MyVector2(int xInput, int yInput)
        {
            X = xInput;
            Y = yInput;
        }
    }
}
