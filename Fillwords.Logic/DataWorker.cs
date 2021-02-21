using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fillwords
{
    static class DataWorker
    {
        static public WordsSet wordsSet;
        static public Dictionary<string, int> userScoreDict = new Dictionary<string, int>();

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

                    wordsSet = new WordsSet(output);
                }
                else
                {
                    Printer.DrawPopupWindow("Добавьте слова в словарь \"words.txt\" (сликом малое количество слов может не позволить сгенерировать поле)");
                    Console.ReadKey(true);
                    Environment.Exit(0);
                }
            }
            else
            {
                File.Create("words.txt");
                Printer.DrawPopupWindow("Добавьте слова в словарь \"words.txt\" (сликом малое количество слов может не позволить сгенерировать поле)");
                Console.ReadKey(true);
                Environment.Exit(0);
            }
        }

        static public void ReadUserScoreFromFile(string path)
        {
            if (File.Exists(path))
            {
                var text = File.ReadLines(path);

                foreach (var word in text)
                    userScoreDict.Add(word.Split(' ')[0], Convert.ToInt32(word.Split(' ')[1]));
            }
            else
            {
                File.Create(path);
            }
        }

        static public void UpdateUsetScoreFile(string path)
        {
            string output = string.Empty;
            foreach (var user in userScoreDict)
            {
                output += user.Key + " " + user.Value + "\n";
            }

            output.Remove(output.Length - 1);

            File.WriteAllText(path, output);
        }

        static public void ReadSettingsFromFile(string path)
        {
            if (File.Exists(path))
            {
                var text = File.ReadLines(path);

                int i = 0;
                foreach (var word in text)
                {
                    if (i != 7) Settings.property[i] = Int32.Parse(word);
                    else Settings.property[i] = bool.Parse(word);
                    i++;
                }
            }
            else
            {
                Settings.SetDefaultSettings();
                UpdateSettingsFile(path);
            }    
        }

        static public void UpdateSettingsFile(string path)
        {
            string output = string.Empty;
            for (int i = 0; i <= Settings.property.lenght; i++)
            {
                output += Settings.property[i] + "\n";
            }

            output.Remove(output.Length - 1);

            File.WriteAllText(path, output);
        }

        static public void SaveField(Field field, string path)
        {
            string data = string.Empty;

            data += " ";
            foreach (string word in field.wordsList)
                data += word + " ";
            data = data.Remove(data.Length - 1);
            data += "\n";

            data += "|/";
            foreach (List<MyVector2> wordCoord in field.wordPos)
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

            data += field.xSize + " " + field.ySize;
            data += "\n";

            for(int y = 0; y < field.ySize; y++)
                for (int x = 0; x < field.xSize; x++)
                    data += field.cellLetter[x,y];
            data += "\n";

            for (int y = 0; y < field.ySize; y++)
                for (int x = 0; x < field.xSize; x++)
                    data += field.cellColor[x, y, 0] + " " + field.cellColor[x, y, 1] + " ";

            File.WriteAllText(path, data);
        }

        static public Field LoadField(string path)
        {
            Field field = new Field();
            field.isLoaded = true;
            string[] data = File.ReadAllLines(path);

            //список слов
            foreach (char letter in data[0])
            {
                if (letter != ' ')
                {
                    field.wordsList[field.wordsList.Count - 1] += letter;
                }
                else
                {
                    field.wordsList.Add(string.Empty);
                }
            }

            //координаты слов
            string localStr = string.Empty;
            foreach (char letter in data[1])
            {
                if (letter == '|')
                {
                    field.wordPos.Add(new List<MyVector2>());
                }
                else if (letter == '/')
                {
                    field.wordPos[field.wordPos.Count - 1].Add(new MyVector2());
                }
                else if (letter == '_')
                {
                    MyVector2 coord = field.wordPos[field.wordPos.Count - 1][field.wordPos[field.wordPos.Count - 1].Count - 1];
                    coord.Y = Convert.ToInt32(localStr);
                    field.wordPos[field.wordPos.Count - 1][field.wordPos[field.wordPos.Count - 1].Count - 1] = coord;
                    localStr = string.Empty;
                }
                else if (letter == ' ')
                {
                    MyVector2 coord = field.wordPos[field.wordPos.Count - 1][field.wordPos[field.wordPos.Count - 1].Count - 1];
                    coord.Y = -10;
                    coord.X = Convert.ToInt32(localStr);
                    field.wordPos[field.wordPos.Count - 1][field.wordPos[field.wordPos.Count - 1].Count - 1] = coord;
                    localStr = string.Empty;
                }
                else
                {
                    localStr += letter;
                }
            }

            //размер поля
            string[] local = data[2].Split(' ');
            field.xSize = Convert.ToInt32(local[0]);
            field.ySize = Convert.ToInt32(local[1]);

            //буквы поля
            field.cellLetter = new char[field.xSize, field.ySize];
            for (int i = 0; i < data[3].Length; i++)
            {
                field.cellLetter[i % field.xSize, i / field.xSize] = data[3][i];
            }

            //Цвета поля
            field.cellColor = new ConsoleColor[field.xSize, field.ySize, 2];
            bool isFirstColor = true;
            localStr = string.Empty;
            int score = 0;
            for (int i = 0; i < data[4].Length; i++)
            {
                if (data[4][i] == ' ')
                {
                    int x = (score / 2) % field.xSize;
                    int y = (score / 2) / field.xSize;
                    field.cellColor[x, y, isFirstColor ? 0 : 1] = GetColorFromName(localStr);
                    localStr = string.Empty;
                    isFirstColor = !isFirstColor;
                    score++;
                }
                else
                {
                    localStr += data[4][i];
                }
            }

            return field;
        }

        static public void SavePlayer(string path)
        {
            string data = string.Empty;

            data += Player.name + "\n";

            data += Player.score + "\n";

            foreach(string word in Player.wordsList)
            {
                data += word + " ";
            }

            File.WriteAllText(path, data);
        }

        static public void LoadPlayer(string path)
        {
            string[] data = File.ReadAllLines(path);

            Player.name = data[0];

            Player.score = Convert.ToInt32(data[1]);

            if (data.Length > 2)
            {
                string localStr = string.Empty;
                foreach (char letter in data[2])
                {
                    if (letter == ' ')
                    {
                        Player.wordsList.Add(localStr);
                        localStr = string.Empty;
                    }
                    else
                    {
                        localStr += letter;
                    }
                }
            }
        }

        static public void DeliteSave(string path1, string path2)
        {
            File.Delete(path1);
            File.Delete(path2);
        }

        static public bool saveExist(string path1, string path2)
        {
            return File.Exists(path1) && File.Exists(path2);
        }

        static private ConsoleColor GetColorFromName(string text)
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

    struct WordsSet
    {
        public string[] allWords;            //Массив из всех слов
        public List<List<string>> wordsSet;  //Массив массивов слов, сгрупперованных по длине
        public WordsSet(string[] input)
        {
            this.allWords = input;
            this.wordsSet = new List<List<string>>();
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i].Length > this.wordsSet.Count - 1)
                    {
                        do
                        {
                            this.wordsSet.Add(new List<string>());
                        } while (this.wordsSet.Count - 1 < input[i].Length);
                    }

                    this.wordsSet[input[i].Length].Add(input[i]);
                }
            }
        }
    }

    struct MyVector2
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
