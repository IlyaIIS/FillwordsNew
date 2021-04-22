using System;
using System.Collections.Generic;

namespace Fillwords
{
    public class Field
    {
        Random rnd = new Random();
        
        public List<string> WordsList { get; set; }              //лист слов на поле
        public List<List<MyVectorInt>> WordPos { get; set; }     //лист координат каждой буквы каждого слова
        public int XSize { get; set; }
        public int YSize { get; set; }                           //размер поля
        public char[,] CellLetter { get; set; }                  //поле букв
        public int[,] CellColor { get; set; }
        public bool IsLoaded { get; set; }

        public Field()
        {
            WordsList = new List<string>();
            WordPos = new List<List<MyVectorInt>>();
            IsLoaded = false;
        }

        public void CreateNewField(int xSize, int ySize, WordsSet words)
        {
            XSize = xSize;
            YSize = ySize;
            CellLetter = new char[XSize, YSize];
            CellColor = new int[XSize, YSize];

            bool[,] preField = CreateFieldOfFreeCells();
            var coordList = new List<MyVectorInt>();

            FillFieldWithWordTemplates(preField, coordList);
            BreakWormIntoWords(words);
            FillFieldWithWords(coordList);
        }
        private bool[,] CreateFieldOfFreeCells()
        {
            bool[,] preField = new bool[XSize + 2, YSize + 2];
            for (int y = 0; y <= YSize + 1; y++)
                for (int x = 0; x <= XSize + 1; x++)
                    if (x == XSize + 1 || y == 0 || x == 0 || y == YSize + 1) 
                        preField[x, y] = false;
                    else                                                      
                        preField[x, y] = true;

            return preField;
        }
        private void FillFieldWithWordTemplates(bool[,] preField, List<MyVectorInt> coordList)
        {
            MyVectorInt starcCoord = GetStartCoord();
            int dir = GetStartDirection(starcCoord.X, starcCoord.Y, preField);

            int[,] numField = new int[XSize, YSize];
            int openCellNum = XSize * YSize;
            int actionMod = 0;
            {
                int x = starcCoord.X;
                int y = starcCoord.Y;
                while (openCellNum != 0)
                {
                    if (preField[x, y])
                    {
                        preField[x, y] = false;
                        openCellNum--;
                        numField[x - 1, y - 1] = XSize * YSize - openCellNum;
                        coordList.Add(new MyVectorInt(x - 1, y - 1));
                    }

                    MyVectorInt coordLocal = GetNextCellCoord(x, y, dir);

                    if (!preField[coordLocal.X, coordLocal.Y])
                    {
                        int oldDir = dir;
                        dir = FindDirection(x, y, preField);
                        if (dir == 0)
                            if (openCellNum > 0)
                                throw new Exception("На поле остались пустые ячейки, до которыйх невозможно добраться");
                            else
                                break;

                        if (actionMod == 0)
                        {
                            if (rnd.Next(3) == 0) actionMod = 1;
                        }
                        else if (actionMod == 1)
                        {
                            if (rnd.Next(3) == 0) actionMod = 0;

                            MyVectorInt coordNext = GetNextCellCoord(x, y, dir);
                            MyVectorInt coordNext2 = GetNextCellCoord(coordNext.X, coordNext.Y, (oldDir + 1) % 4 + 1);

                            if (preField[coordNext2.X, coordNext2.Y])
                            {
                                preField[coordNext.X, coordNext.Y] = false;
                                openCellNum -= 1;
                                numField[coordNext.X - 1, coordNext.Y - 1] = XSize * YSize - openCellNum;
                                coordList.Add(new MyVectorInt(coordNext.X - 1, coordNext.Y - 1));
                                x = coordNext2.X;
                                y = coordNext2.Y;
                                dir = (oldDir + 1) % 4 + 1;
                            }
                            else
                            {
                                actionMod = 0;
                            }
                        }
                    }
                    else
                    {
                        x = coordLocal.X;
                        y = coordLocal.Y;
                    }
                }
            }
        }
        private void BreakWormIntoWords(WordsSet words)
        {
            int cellNum;
            List<int> wordsLenghtList;
            int tryNum = 0;
            do
            {
                WordsList = new List<string>();
                cellNum = XSize * YSize;
                List<int> wordsLenghtNum = new List<int>();
                wordsLenghtList = new List<int>();

                for (int i = 0; i < words.WordsSetList.Count; i++)
                    wordsLenghtNum.Add(words.WordsSetList[i].Count);

                do
                {
                    int wordLenght;
                    int i = 0;
                    do
                    {
                        i++;
                        if (i >= 50 || cellNum <= 2)
                        {
                            wordLenght = 0;
                            break;
                        }
                        wordLenght = rnd.Next(Math.Min(wordsLenghtNum.Count - 1, cellNum) - 2) + 3;
                        if (wordLenght < 5 || wordLenght > 8) wordLenght = rnd.Next(Math.Min(wordsLenghtNum.Count - 1, cellNum) - 2) + 3;
                        if (wordLenght < 5 || wordLenght > 8) wordLenght = rnd.Next(Math.Min(wordsLenghtNum.Count - 1, cellNum) - 2) + 3;
                    } while (wordsLenghtNum[wordLenght] == 0);

                    if (wordLenght == 0) break;

                    wordsLenghtNum[wordLenght]--;
                    cellNum -= wordLenght;
                    wordsLenghtList.Add(wordLenght);
                    WordsList.Add(words.WordsSetList[wordLenght][rnd.Next(words.WordsSetList[wordLenght].Count)]);
                } while (cellNum > 0);
                if (cellNum == 0 && WordsList.Count < Settings.WordNumMin) continue;

                tryNum++;
                if (tryNum > 10000) Environment.Exit(0);
            } while (cellNum > 0);
        }
        private void FillFieldWithWords(List<MyVectorInt> coordList)
        {
            int step = 0;
            for (int wordNum = 0; wordNum < WordsList.Count; wordNum++)
            {
                WordPos.Add(new List<MyVectorInt>());
                for (int letterNum = 0; letterNum < WordsList[wordNum].Length; letterNum++)
                {
                    char letter = WordsList[wordNum][letterNum];
                    int x = coordList[step].X;
                    int y = coordList[step].Y;
                    WordPos[wordNum].Add(coordList[step]);
                    CellLetter[x, y] = letter;
                    CellColor[x, y] = Settings.FieldColor;
                    step++;
                }
            }
        }

        private MyVectorInt GetStartCoord()
        {
            MyVectorInt output = new MyVectorInt();
            if (rnd.Next(2) == 0)
            {
                if (rnd.Next(2) == 0)
                {
                    output.X = 1;
                    output.Y = 1;
                }
                else
                {
                    output.X = 1;
                    output.Y = YSize;
                }
            }
            else
            {
                if (rnd.Next(2) == 0)
                {
                    output.X = XSize;
                    output.Y = 1;
                }
                else
                {
                    output.X = XSize;
                    output.Y = YSize;
                }
            }

            return output;
        }

        public int FindDirection(int x, int y, bool[,] field)
        {
            List<int> dirList = new List<int>() { 1, 2, 3, 4 };

            if (!field[x + 1, y    ]) dirList.Remove(1);
            if (!field[x    , y - 1]) dirList.Remove(2);
            if (!field[x - 1, y    ]) dirList.Remove(3);
            if (!field[x    , y + 1]) dirList.Remove(4);

            if (dirList.Count != 0) 
                return dirList[rnd.Next(dirList.Count)];
            else 
                return 0;
        }

        private int GetStartDirection(int x, int y, bool[,] field)
        {
            int output = 0;
            if (!field[x + 1, y]) output = (rnd.Next(2) == 0) ? 2 : 4;
            if (!field[x, y - 1]) output = (rnd.Next(2) == 0) ? 1 : 3;
            if (!field[x - 1, y]) output = (rnd.Next(2) == 0) ? 2 : 4;
            if (!field[x, y + 1]) output = (rnd.Next(2) == 0) ? 1 : 3;

            return output;
        }

        private MyVectorInt GetNextCellCoord(int x, int y, int dir)
        {
            return new MyVectorInt( x + (-(dir - 2) % 2), y + ((dir - 3) % 2) );
        }
    }
}
