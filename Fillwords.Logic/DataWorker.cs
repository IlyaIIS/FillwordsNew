using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fillwords
{
    public static class DataWorker
    {
        static public WordsSet WordsSet { get; private set; }
        static public Dictionary<string, int> UserScoreDict { get; private set; } = new Dictionary<string, int>();
        static public readonly string UserScoreSavePath = "users_score.txt";
        static public readonly string FieldSavePath = "field_save.txt";
        static public readonly string PlayerSavePath = "player_save.txt";
        static public readonly string WordsDictionaryPath = "words.txt";
        static public readonly string SettingsSavePath = "settings.txt";

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

                UserScoreDict = UserScoreDict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
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
            foreach (List<MyVectorInt> wordCoord in field.WordPos)
            {
                foreach (MyVectorInt coord in wordCoord)
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

            ReadAndSetFieldWordList(field, data[0]);
            ReadAndSetFieldWordCoords(field, data[1]);
            ReadAndSetFieldSize(field, data[2]);
            ReadAndSetFieldLetters(field, data[3]);
            ReadAndSetFieldColors(field, data[4]);

            return field;
        }

        private static void ReadAndSetFieldWordList(Field field, string line)
        {
            foreach (char letter in line)
                if (letter != ' ')
                    field.WordsList[field.WordsList.Count - 1] += letter;
                else
                    field.WordsList.Add(string.Empty);
        }
        private static void ReadAndSetFieldWordCoords(Field field, string line)
        {
            string localStr = string.Empty;
            foreach (char letter in line)
            {
                if (letter == '|')
                {
                    field.WordPos.Add(new List<MyVectorInt>());
                }
                else if (letter == '/')
                {
                    field.WordPos[field.WordPos.Count - 1].Add(new MyVectorInt());
                }
                else if (letter == '_')
                {
                    MyVectorInt coord = field.WordPos[field.WordPos.Count - 1][field.WordPos[field.WordPos.Count - 1].Count - 1];
                    coord.Y = Convert.ToInt32(localStr);
                    field.WordPos[field.WordPos.Count - 1][field.WordPos[field.WordPos.Count - 1].Count - 1] = coord;
                    localStr = string.Empty;
                }
                else if (letter == ' ')
                {
                    MyVectorInt coord = field.WordPos[field.WordPos.Count - 1][field.WordPos[field.WordPos.Count - 1].Count - 1];
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
        }
        private static void ReadAndSetFieldSize(Field field, string line)
        {
            string[] local = line.Split(' ');
            field.XSize = Convert.ToInt32(local[0]);
            field.YSize = Convert.ToInt32(local[1]);
        }
        private static void ReadAndSetFieldLetters(Field field, string line)
        {
            field.CellLetter = new char[field.XSize, field.YSize];
            for (int i = 0; i < line.Length; i++)
            {
                field.CellLetter[i % field.XSize, i / field.XSize] = line[i];
            }
        }
        private static void ReadAndSetFieldColors(Field field, string line)
        {
            field.CellColor = new int[field.XSize, field.YSize];
            string localStr = string.Empty;
            int score = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    int x = score % field.XSize;
                    int y = score / field.XSize;
                    field.CellColor[x, y] = Int32.Parse(localStr);
                    localStr = string.Empty;
                    score++;
                }
                else
                {
                    localStr += line[i];
                }
            }
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

        public static void DeleteSave(string path)
        {
            File.Delete(path);
        }

        public static bool IsSaveExist(string path1)
        {
            return File.Exists(path1);
        }
    }

    public struct WordsSet
    {
        public string[] AllWords { get; private set; }                
        public List<List<string>> WordsSetList { get; private set; }  //Список списков слов, сгрупперованных по длине
        public WordsSet(string[] input)
        {
            AllWords = input;
            WordsSetList = new List<List<string>>();
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i].Length > WordsSetList.Count - 1)
                    {
                        do
                        {
                            WordsSetList.Add(new List<string>());
                        } while (WordsSetList.Count - 1 < input[i].Length);
                    }

                    WordsSetList[input[i].Length].Add(input[i]);
                }
            }
        }
    }

    public struct MyVectorInt
    {
        public int X { get; set; }
        public int Y { get; set; }

        public MyVectorInt(int xInput, int yInput)
        {
            X = xInput;
            Y = yInput;
        }
    }
}
